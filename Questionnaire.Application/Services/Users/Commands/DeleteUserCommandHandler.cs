using Questionnaire.DataAccess.UnitOfWork;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ErrorMessages;
using Questionnaire.SharedKernel.Exceptions;
using Questionnaire.SharedKernel.ModelDtos.Users.Commands;

namespace Questionnaire.Application.Services.Users.Commands;

public class DeleteUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.User.GetAsync(request.Id, cancellationToken)
                ?? throw new CustomException(Resource.The_desired_information_does_not_found);

            await _unitOfWork.User.DeleteAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new DeleteUserResponse(Success: true);
        }
        catch (CustomException ex)
        {
            throw new CustomException(ex.Message);
        }
    }
}