using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Questionnaire.DataAccess.Context;
using Questionnaire.DataAccess.DomainRepositories;
using Questionnaire.DataAccess.Models;
using Questionnaire.DataAccess.UnitOfWork;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.Services;

namespace Questionnaire.Application.Registers;

public static class Services
{
    public static IServiceCollection CollectRepos(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<IDispatcher, Dispatcher>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.BuildServiceProvider();

        return services;
    }
}
