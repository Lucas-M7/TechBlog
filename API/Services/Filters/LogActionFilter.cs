using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Services.Filters;

public class LogActionFilter(ILogger<LogActionFilter> logger) : IActionFilter
{
    private readonly ILogger<LogActionFilter> _logger = logger;

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("Executed action: " + context.ActionDescriptor.DisplayName);
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("Executing action: " + context.ActionDescriptor.DisplayName);
    }
}