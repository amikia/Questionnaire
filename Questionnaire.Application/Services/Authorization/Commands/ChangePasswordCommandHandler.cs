using Microsoft.AspNetCore.Identity;
using Questionnaire.DataAccess.Models;
using Questionnaire.DataAccess.UnitOfWork;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.Exceptions;

namespace Questionnaire.Application.Services.Authorization.Commands;

public class ChangePasswordCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
    : IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken = default)
    {
        try 
        {
            var currentUser = await _unitOfWork.User.GetCurrentUserAsync(cancellationToken);

            var newPassword = _passwordHasher.HashPassword(currentUser, request.Password);

            currentUser.PasswordHash = newPassword;

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
        catch (CustomException ex)
        {
            throw new CustomException(ex.Message);
        }
    }
}
