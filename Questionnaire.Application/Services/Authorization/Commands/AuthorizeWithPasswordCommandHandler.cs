using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Questionnaire.DataAccess.Models;
using Questionnaire.DataAccess.UnitOfWork;
using Questionnaire.SharedKernel.Classes;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ErrorMessages;
using Questionnaire.SharedKernel.Exceptions;
using Questionnaire.SharedKernel.Services;

namespace Questionnaire.Application.Services.Authorization.Commands;

public class AuthorizeWithPasswordCommandHandler(
    IUnitOfWork unitOfWork, 
    IPasswordHasher<User> passwordHasher, 
    IJwtService jwtService,
    IHttpContextAccessor httpContextAccessor) 
    : IRequestHandler<AuthorizeWithPasswordCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<bool> Handle(AuthorizeWithPasswordCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.User.GetByUserNameAsync(request.Username, cancellationToken)
                ?? throw new CustomException(Resource.The_desired_information_does_not_found);

            var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (passwordResult == PasswordVerificationResult.Failed) 
                throw new CustomException(Resource.The_desired_information_does_not_found);

            var token = _jwtService.GenerateToken(user.Id.ToString(), user.Username, user.PhoneNumber);

            _httpContextAccessor.HttpContext!.Response.Cookies.Append(
                StaticVariable.tokenName,
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });

            return true;

        }
        catch (CustomException ex)
        {
            throw new CustomException(ex.Message);
        }
    }
}