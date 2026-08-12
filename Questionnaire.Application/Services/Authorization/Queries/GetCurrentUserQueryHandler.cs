using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.Exceptions;
using Questionnaire.SharedKernel.ModelDtos.Authorization.Queries;

namespace Questionnaire.Application.Services.Authorization.Queries;

public class GetCurrentUserQueryHandler() : IRequestHandler<GetCurrentUserQuery, GetCurrentUserDto>
{
    public Task<GetCurrentUserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (CustomException ex)
        {
            throw new CustomException(ex.Message);
        }
    }
}
