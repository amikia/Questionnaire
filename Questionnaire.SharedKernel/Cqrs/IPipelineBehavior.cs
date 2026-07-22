using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Questionnaire.SharedKernel.Exceptions;

namespace Questionnaire.SharedKernel.Cqrs;

public interface IPipelineBehavior<TRequest, TResult> where TRequest : IRequest<TResult>
{
    Task<TResult> HandleAsync(TRequest request, Func<Task<TResult>> next, CancellationToken cancellationToken);
}

public class ValidationBehavior<TRequest, TResponse>(IServiceProvider serviceProvider) : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken)
    {
        var validator = serviceProvider.GetService<IValidator<TRequest>>();
        if (validator == null)
        {
            return await next();
        }

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.FirstOrDefault();

            throw new CustomException(error is not null ? string.Concat(error.PropertyName, " : ", error.ErrorMessage) : null);
        }

        return await next();
    }
}