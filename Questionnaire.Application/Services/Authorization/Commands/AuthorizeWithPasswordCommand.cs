using FluentValidation;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ErrorMessages;

namespace Questionnaire.Application.Services.Authorization.Commands;

public record AuthorizeWithPasswordCommand(string Username, string Password) : IRequest<bool>;

public sealed class AuthorizeWithPasswordCommandValidator : AbstractValidator<AuthorizeWithPasswordCommand>
{
    public AuthorizeWithPasswordCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);
    }
}