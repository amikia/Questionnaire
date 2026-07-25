using Microsoft.Extensions.DependencyInjection;
using Questionnaire.SharedKernel.Cqrs;
using System.Reflection;

namespace Questionnaire.Application.Registers;

public static class Handlers
{
    public static IServiceCollection RegisterRequestHandlers(this IServiceCollection services, params Assembly[] assemblies)
    {
        var targetAssemblies = assemblies.Length > 0
            ? assemblies
            : new[] { Assembly.GetExecutingAssembly() };

        var handlerTypes = targetAssemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .Select(i => new { Interface = i, Implementation = t }));

        foreach (var handler in handlerTypes)
        {
            services.AddScoped(handler.Interface, handler.Implementation);
        }

        return services;
    }
}