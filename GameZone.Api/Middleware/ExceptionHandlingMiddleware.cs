using GameZone.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Api.Middleware;
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            _logger.LogInformation("Request was cancelled by the client.");
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (status, title, detail) = ex switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Not found", ex.Message),
            ConflictException => (StatusCodes.Status409Conflict, "Conflict", ex.Message),
            BadRequestException => (StatusCodes.Status400BadRequest, "Bad request", ex.Message),
            UnauthorizedException => (StatusCodes.Status401Unauthorized, "Unauthorized", ex.Message),
            _ => (StatusCodes.Status500InternalServerError, "Server error", "An unexpected error occurred.")
        };

        if (status == StatusCodes.Status500InternalServerError)
            _logger.LogError(ex, "Unhandled exception on {Path}", context.Request.Path);
        else
            _logger.LogWarning("Handled {Type} on {Path}: {Message}", ex.GetType().Name, context.Request.Path, ex.Message);

        if (context.Response.HasStarted) return;

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        });
    }
}