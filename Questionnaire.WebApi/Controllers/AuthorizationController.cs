using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questionnaire.Application.Services.Authorization.Commands;
using Questionnaire.Application.Services.Authorization.Queries;
using Questionnaire.SharedKernel.Classes;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ModelDtos.Authorization.Queries;
using Questionnaire.WebApi.Controllers.Base;

namespace Questionnaire.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController(IDispatcher dispatcher, ILogger logger) : BaseController(dispatcher, logger)
{
    [HttpPost]
    [Route(nameof(AuthorizeWithPassword))]
    public async Task<ActionResult<ApiResponse<bool>>> AuthorizeWithPassword(AuthorizeWithPasswordCommand request, CancellationToken cancellationToken)
    {
        return ApiResponse<bool>.Success(
            await _dispatcher.Dispatch<AuthorizeWithPasswordCommand, bool>(
                request, cancellationToken));
    }

    [Authorize]
    [HttpPost]
    [Route(nameof(LogOut))]
    public async Task<ActionResult<ApiResponse<bool>>> LogOut(CancellationToken cancellationToken)
    {
        return ApiResponse<bool>.Success(
            await _dispatcher.Dispatch<LogOutCommand, bool>(
                new LogOutCommand(), cancellationToken));
    }

    [Authorize]
    [HttpGet]
    [Route(nameof(GetCurrentUser))]
    public async Task<ActionResult<ApiResponse<GetCurrentUserDto>>> GetCurrentUser(CancellationToken cancellationToken)
    {
        return ApiResponse<GetCurrentUserDto>.Success(
            await _dispatcher.Dispatch<GetCurrentUserQuery, GetCurrentUserDto>(
                new GetCurrentUserQuery(), cancellationToken));
    }
}