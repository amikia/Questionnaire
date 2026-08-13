using FluentValidation;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ErrorMessages;

namespace Questionnaire.Application.Services.Authorization.Commands;

public record ChangePasswordCommand(string Password) : IRequest<bool>;

public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);
    }
}