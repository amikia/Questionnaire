using Questionnaire.DataAccess.UnitOfWork;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ErrorMessages;
using Questionnaire.SharedKernel.Exceptions;
using Questionnaire.SharedKernel.ModelDtos.Users.Queries;

namespace Questionnaire.Application.Services.Users.Queries;

public class GetUserQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserQuery, GetUserDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<GetUserDto> Handle(GetUserQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.User.GetAsync(request.Id, cancellationToken)
                ?? throw new CustomException(Resource.The_desired_information_does_not_found);

            return new GetUserDto(user.Id.ToString(), user.Username, user.FullName(), user.PhoneNumber);
        }
        catch (CustomException ex)
        {
            throw new CustomException(ex.Message);
        }
    }
}
