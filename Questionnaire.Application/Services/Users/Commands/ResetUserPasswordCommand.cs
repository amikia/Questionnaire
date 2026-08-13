using FluentValidation;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ErrorMessages;

namespace Questionnaire.Application.Services.Users.Commands;

public record ResetUserPasswordCommand(Guid Id) : IRequest<bool>;

public sealed class ResetUserPasswordCommandValidator : AbstractValidator<ResetUserPasswordCommand>
{
    public ResetUserPasswordCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);
    }
}