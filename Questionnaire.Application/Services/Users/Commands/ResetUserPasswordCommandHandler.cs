using Microsoft.AspNetCore.Identity;
using Questionnaire.DataAccess.Models;
using Questionnaire.DataAccess.UnitOfWork;
using Questionnaire.SharedKernel.Classes;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ErrorMessages;
using Questionnaire.SharedKernel.Exceptions;

namespace Questionnaire.Application.Services.Users.Commands;

public class ResetUserPasswordCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
    : IRequestHandler<ResetUserPasswordCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    public async Task<bool> Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.User.GetAsync(request.Id, cancellationToken)
                ?? throw new CustomException(Resource.The_desired_information_does_not_found);

            var defaultPassword = _passwordHasher.HashPassword(user, StaticVariable.defaultPassword);

            user.PasswordHash = defaultPassword;

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
        catch(CustomException ex)
        {
            throw new CustomException(ex.Message);
        }
    }
}
