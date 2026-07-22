using Microsoft.Extensions.DependencyInjection;

namespace Questionnaire.SharedKernel.Cqrs;

public interface IDispatcher
{
    Task<TResult> Dispatch<TRequest, TResult>(TRequest request, CancellationToken cancellationToken = default) 
        where TRequest : IRequest<TResult>;
}

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TResult> Dispatch<TRequest, TResult>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResult>
    {
        var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResult>>();

        var behaviors = _serviceProvider.GetServices<IPipelineBehavior<TRequest, TResult>>().Reverse();

        Func<Task<TResult>> handlerDelegate = () => handler.Handle(request, cancellationToken);

        foreach (var behavior in behaviors)
        {
            var next = handlerDelegate;
            handlerDelegate = () => behavior.HandleAsync(request, next, cancellationToken);
        }

        return await handlerDelegate();
    }
}