using FluentValidation;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ErrorMessages;
using Questionnaire.SharedKernel.ModelDtos.Users.Commands;

namespace Questionnaire.Application.Services.Users.Commands;

public record CreateUserCommand(string Username, string Firstname, string Lastname, string PhoneNumber, string Password) 
    : IRequest<CreateUserResponse>;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);

        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);

        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);
    }
}