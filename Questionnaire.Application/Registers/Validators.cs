using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Questionnaire.Application.Services.Authorization.Commands;

namespace Questionnaire.Application.Registers;

public static class Validators
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(AuthorizeWithPasswordCommandValidator).Assembly);
        return services;
    }
}
