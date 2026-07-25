using FluentValidation;

namespace Questionnaire.Application.Services.Authorization.Commands;

public record AuthorizeWithPasswordCommand();

public sealed class AuthorizeWithPasswordCommandValidator : AbstractValidator<AuthorizeWithPasswordCommand>
{
    public AuthorizeWithPasswordCommandValidator()
    {
        
    }
}