using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questionnaire.Application.Services.Users.Commands;
using Questionnaire.SharedKernel.Classes;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ModelDtos.Users.Commands;
using Questionnaire.WebApi.Controllers.Base;

namespace Questionnaire.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IDispatcher dispatcher, ILogger logger) : BaseController(dispatcher, logger)
{
    [Authorize]
    [HttpPost]
    [Route(nameof(Create))]
    public async Task<ActionResult<ApiResponse<CreateUserResponse>>> Create(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return ApiResponse<CreateUserResponse>.Success(
            await _dispatcher.Dispatch<CreateUserCommand, CreateUserResponse>(
                request, cancellationToken));
    }
}