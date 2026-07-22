using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Questionnaire.SharedKernel.Classes;
using Questionnaire.SharedKernel.Exceptions;
using System.Net;
using System.Text.Json;

namespace Questionnaire.SharedKernel.Middlewares;

public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger, IHostEnvironment env)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger = logger;
    private readonly IHostEnvironment _env = env;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            await HandleExceptionAsync(context, error);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception error)
    {
        if (context.Response.HasStarted)
        {
            _logger.LogWarning(error, "Exception occurred after response had already started.");
            throw error;
        }

        var (statusCode, publicMessage, logLevel) = MapException(error);

        _logger.Log(logLevel, error, "Request failed with {StatusCode}: {Message}", (int)statusCode, error.Message);

        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)statusCode;

        var responseModel = ApiResponse<string>.Fail(
            _env.IsDevelopment() ? error.Message : publicMessage);

        var json = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await response.WriteAsync(json, context.RequestAborted);
    }

    private static (HttpStatusCode StatusCode, string Message, LogLevel Level) MapException(Exception error) => error switch
    {
        CustomException => (HttpStatusCode.BadRequest, error.Message, LogLevel.Warning),
        KeyNotFoundException => (HttpStatusCode.NotFound, "Not Found", LogLevel.Information),
        PaymentException => (HttpStatusCode.PaymentRequired, "Payment Required", LogLevel.Warning),
        UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized", LogLevel.Warning),
        InvalidOperationException => (HttpStatusCode.Conflict, "Invalid Operation", LogLevel.Warning),
        _ => (HttpStatusCode.InternalServerError, "Internal Server Error", LogLevel.Error),
    };
}