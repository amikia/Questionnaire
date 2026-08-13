using Microsoft.AspNetCore.Http;
using Questionnaire.SharedKernel.Classes;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.Exceptions;

namespace Questionnaire.Application.Services.Authorization.Commands;

public class LogOutCommandHandler(IHttpContextAccessor httpContextAccessor) : IRequestHandler<LogOutCommand, bool>
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public async Task<bool> Handle(LogOutCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(StaticVariable.tokenName);

            return true;
        }
        catch (CustomException ex)
        {
            throw new CustomException(ex.Message);
        }
    }
}
