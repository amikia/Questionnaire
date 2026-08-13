using Questionnaire.DataAccess.Models;
using Questionnaire.DataAccess.UnitOfWork;
using Questionnaire.SharedKernel.Cqrs;
using Questionnaire.SharedKernel.ErrorMessages;
using Questionnaire.SharedKernel.Exceptions;
using Questionnaire.SharedKernel.ModelDtos.Users.Commands;
using Questionnaire.SharedKernel.Services;

namespace Questionnaire.Application.Services.Users.Commands;

public class UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMappingService mappingService)
    : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMappingService _mappingService = mappingService;

    public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.User.GetAsync(request.Id, cancellationToken)
                ?? throw new CustomException(Resource.The_desired_information_does_not_found);

            _mappingService.Map<UpdateUserCommand, User>(request, user);

            await _unitOfWork.CommitAsync(cancellationToken);

            return new UpdateUserResponse(user.Id);
        }
        catch (CustomException ex)
        {
            throw new CustomException(ex.Message);
        }
    }
}
