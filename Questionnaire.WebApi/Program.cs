using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Questionnaire.Application.Registers;
using Questionnaire.DataAccess.Context;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.Middlewares;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region CORS
const string AllowAllHeadersPolicy = "AllowAllPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowAllHeadersPolicy, builder =>
        builder.SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});
#endregion

#region SqlServerConfig
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>()!);
#endregion

#region JwtCookie

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["access_token"];

                return Task.CompletedTask;
            }
        };
    });

#endregion 

#region Logger

builder.Services.AddSingleton<ILogger>(sp =>
{
    var factory = sp.GetRequiredService<ILoggerFactory>();
    return factory.CreateLogger("Application");
});

#endregion

builder.Services.CollectRepos();

builder.Services.RegisterRequestHandlers();

builder.Services.RegisterMappingService();

builder.Services.AddValidators();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();


var app = builder.Build();


#region SwaggerConfiguration

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DefaultModelExpandDepth(depth: -1);
    c.DefaultModelRendering(ModelRendering.Example);
    c.DocExpansion(DocExpansion.None);

});

#endregion


app.UseCors(AllowAllHeadersPolicy);

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();