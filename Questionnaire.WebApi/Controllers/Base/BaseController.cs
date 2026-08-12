using Microsoft.AspNetCore.Mvc;
using Questionnaire.SharedKernel.Cqrs;

namespace Questionnaire.WebApi.Controllers.Base;

[ApiController]
public class BaseController(IDispatcher queryDispatcher, ILogger logger) : ControllerBase
{
    protected readonly IDispatcher _dispatcher = queryDispatcher;
    protected readonly ILogger _logger = logger;
}