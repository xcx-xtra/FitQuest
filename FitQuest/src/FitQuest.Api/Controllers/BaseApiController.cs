using Microsoft.AspNetCore.Mvc;
using FitQuest.Api.Services;

namespace FitQuest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected readonly ILogger Logger;
    protected readonly IDatabaseErrorHandler DatabaseErrorHandler;

    protected BaseApiController(ILogger logger, IDatabaseErrorHandler databaseErrorHandler)
    {
        Logger = logger;
        DatabaseErrorHandler = databaseErrorHandler;
    }

    protected IActionResult HandleException(Exception ex, string operation)
    {
        Logger.LogError(ex, "Error occurred during {Operation}", operation);

        return ex switch
        {
            ArgumentException => BadRequest(new { 
                message = "Invalid request parameters", 
                operation,
                correlationId = HttpContext.TraceIdentifier 
            }),
            
            UnauthorizedAccessException => Unauthorized(new { 
                message = "Unauthorized access", 
                operation,
                correlationId = HttpContext.TraceIdentifier 
            }),
            
            KeyNotFoundException => NotFound(new { 
                message = "Resource not found", 
                operation,
                correlationId = HttpContext.TraceIdentifier 
            }),
            
            InvalidOperationException => Conflict(new { 
                message = "Operation cannot be completed", 
                operation,
                correlationId = HttpContext.TraceIdentifier 
            }),
            
            _ => StatusCode(500, new { 
                message = "An internal server error occurred", 
                operation,
                correlationId = HttpContext.TraceIdentifier 
            })
        };
    }

    protected async Task<IActionResult> ExecuteWithErrorHandlingAsync<T>(
        Func<Task<T>> operation, 
        string operationName)
    {
        try
        {
            var result = await operation();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex, operationName);
        }
    }

    protected IActionResult ExecuteWithErrorHandling<T>(
        Func<T> operation, 
        string operationName)
    {
        try
        {
            var result = operation();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex, operationName);
        }
    }
}