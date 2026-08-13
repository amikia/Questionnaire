using Questionnaire.DataAccess.UnitOfWork;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.Exceptions;
using Questionnaire.SharedKernel.ModelDtos.Authorization.Queries;

namespace Questionnaire.Application.Services.Authorization.Queries;

public class GetCurrentUserQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCurrentUserQuery, GetCurrentUserDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetCurrentUserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            var currentUser = await _unitOfWork.User.GetCurrentUserAsync(cancellationToken);

            return new GetCurrentUserDto(currentUser.Id.ToString(), currentUser.Username, currentUser.FullName());
        }
        catch (CustomException ex)
        {
            throw new CustomException(ex.Message);
        }
    }
}
