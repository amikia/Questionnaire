using Microsoft.Extensions.DependencyInjection;
using Questionnaire.DataAccess.Context;

namespace Questionnaire.Application.Registers;

public static class Services
{
    public static IServiceCollection CollectRepos(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();
        
        
        services.BuildServiceProvider();

        return services;
    }
}
