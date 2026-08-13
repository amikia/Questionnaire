using FluentValidation;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ModelDtos.Users.Queries;

namespace Questionnaire.Application.Services.Users.Queries;

public record GetUsersQuery() : IRequest<List<GetUsersDto>>;

public sealed class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        
    }
}