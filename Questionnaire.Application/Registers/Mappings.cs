using Microsoft.Extensions.DependencyInjection;
using Questionnaire.DataAccess;
using Questionnaire.SharedKernel.Services;
using System.Reflection;

namespace Questionnaire.Application.Registers;

public static class Mappings
{
    public static IServiceCollection RegisterMappingService(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (assemblies is null || assemblies.Length == 0)
            assemblies = new[] { Assembly.GetCallingAssembly() };

        var mapperInterfaceType = typeof(IMapper<,>);

        foreach (var assembly in assemblies)
        {
            var mapperImplementations = assembly.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false })
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == mapperInterfaceType)
                    .Select(i => new { Interface = i, Implementation = t }));

            foreach (var map in mapperImplementations)
                services.AddTransient(map.Interface, map.Implementation);
        }

        services.AddSingleton<IMappingService, MappingService>();
        return services;
    }
}