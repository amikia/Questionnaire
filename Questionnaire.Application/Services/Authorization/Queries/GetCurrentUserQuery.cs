using FluentValidation;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ModelDtos.Authorization.Queries;

namespace Questionnaire.Application.Services.Authorization.Queries;

public record GetCurrentUserQuery() : IRequest<GetCurrentUserDto>;

public sealed class GetCurrentUserQueryValidator : AbstractValidator<GetCurrentUserQuery>
{
    public GetCurrentUserQueryValidator()
    {
        
    }
}