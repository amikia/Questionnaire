using FluentValidation;
using Questionnaire.SharedKernel.Cqrs;

namespace Questionnaire.Application.Services.Authorization.Commands;

public record LogOutCommand() : IRequest<bool>;

public sealed class LogOutCommandValidator : AbstractValidator<LogOutCommand>
{
	public LogOutCommandValidator()
	{
		
	}
}