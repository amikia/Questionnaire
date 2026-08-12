using Microsoft.AspNetCore.Identity;
using Questionnaire.DataAccess.Models;
using Questionnaire.DataAccess.UnitOfWork;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.Exceptions;
using Questionnaire.SharedKernel.ModelDtos.Users.Commands;
using Questionnaire.SharedKernel.Services;

namespace Questionnaire.Application.Services.Users.Commands;

public class CreateUserCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher, IMappingService mappingService) : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IMappingService _mappingService = mappingService;

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var model = _mappingService.Map<CreateUserCommand, User>(request);

            var user = await _unitOfWork.User.AddAsync(model, cancellationToken);

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _unitOfWork.CommitAsync(cancellationToken);

            return new CreateUserResponse(user.Id);
        }
        catch (CustomException ex)
        {
            throw new CustomException(ex.Message);
        }
    }
}
