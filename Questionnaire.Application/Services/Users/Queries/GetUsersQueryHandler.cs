using Questionnaire.DataAccess.UnitOfWork;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.Exceptions;
using Questionnaire.SharedKernel.ModelDtos.Users.Queries;

namespace Questionnaire.Application.Services.Users.Queries;

public class GetUsersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUsersQuery, List<GetUsersDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<GetUsersDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await _unitOfWork.User.GetAllAsync(cancellationToken);

            return [.. users.Where(x => !x.IsDeleted).Select(x => new GetUsersDto(x.Id.ToString(), x.FullName()))];
        }
        catch (CustomException ex)
        {
            throw new CustomException(ex.Message);
        }
    }
}
