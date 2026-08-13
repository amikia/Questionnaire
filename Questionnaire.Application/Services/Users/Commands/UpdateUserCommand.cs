using FluentValidation;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ErrorMessages;
using Questionnaire.SharedKernel.ModelDtos.Users.Commands;

namespace Questionnaire.Application.Services.Users.Commands;

public record UpdateUserCommand(Guid Id, string Username, string Firstname, string Lastname, string PhoneNumber) 
    : IRequest<UpdateUserResponse>;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);

        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);

        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);
    }
}