using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questionnaire.Application.Services.Users.Commands;
using Questionnaire.Application.Services.Users.Queries;
using Questionnaire.SharedKernel.Classes;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ModelDtos.Users.Commands;
using Questionnaire.SharedKernel.ModelDtos.Users.Queries;
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

    [Authorize]
    [HttpPost]
    [Route(nameof(Delete))]
    public async Task<ActionResult<ApiResponse<DeleteUserResponse>>> Delete(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        return ApiResponse<DeleteUserResponse>.Success(
            await _dispatcher.Dispatch<DeleteUserCommand, DeleteUserResponse>(
                request, cancellationToken));
    }

    [Authorize]
    [HttpPost]
    [Route(nameof(Update))]
    public async Task<ActionResult<ApiResponse<UpdateUserResponse>>> Update(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        return ApiResponse<UpdateUserResponse>.Success(
            await _dispatcher.Dispatch<UpdateUserCommand, UpdateUserResponse>(
                request, cancellationToken));
    }

    [Authorize]
    [HttpGet]
    [Route(nameof(Get) + "/{id:guid}")]
    public async Task<ActionResult<ApiResponse<GetUserDto>>> Get(Guid id, CancellationToken cancellationToken)
    {
        return ApiResponse<GetUserDto>.Success(
            await _dispatcher.Dispatch<GetUserQuery, GetUserDto>(
                new GetUserQuery(id), cancellationToken));
    }

    [Authorize]
    [HttpGet]
    [Route(nameof(GetAll))]
    public async Task<ActionResult<ApiResponse<List<GetUsersDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return ApiResponse<List<GetUsersDto>>.Success(
            await _dispatcher.Dispatch<GetUsersQuery, List<GetUsersDto>>(
                new GetUsersQuery(), cancellationToken));
    }
}