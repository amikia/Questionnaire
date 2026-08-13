using FluentValidation;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ErrorMessages;
using Questionnaire.SharedKernel.ModelDtos.Users.Queries;

namespace Questionnaire.Application.Services.Users.Queries;

public record GetUserQuery(Guid Id) : IRequest<GetUserDto>;

public sealed class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Resource.Entering_this_field_is_required);
    }
}