using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.Exceptions;

namespace Questionnaire.Application.Services.Authorization.Commands;

public class LogOutCommandHandler() : IRequestHandler<LogOutCommand, bool>
{
    public async Task<bool> Handle(LogOutCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (CustomException ex)
        {
            throw new CustomException(ex.Message);
        }
    }
}
