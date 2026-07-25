using Microsoft.EntityFrameworkCore;
using Questionnaire.Application.Registers;
using Questionnaire.DataAccess.Context;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.Middlewares;
using Swashbuckle.AspNetCore.SwaggerUI;

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

builder.Services.CollectRepos();
builder.Services.RegisterRequestHandlers();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DefaultModelExpandDepth(depth: -1);
    c.DefaultModelRendering(ModelRendering.Example);
    c.DocExpansion(DocExpansion.None);

});

app.UseCors(AllowAllHeadersPolicy);

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();