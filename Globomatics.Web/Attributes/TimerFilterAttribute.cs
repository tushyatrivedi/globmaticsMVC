using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Globomatics.Web.Attributes
{
    public class TimerFilterAttribute:ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //code before executing the action
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<TimerFilterAttribute>>();

            logger.LogInformation($"Starting execution of {context.ActionDescriptor.DisplayName}");

            //starting the stopwatch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //Execute the action
            await next();

            //code after executing the action
            stopwatch.Stop();
            logger.LogInformation($"{context.ActionDescriptor.DisplayName} took {stopwatch.ElapsedMilliseconds} ms to complete!");
        }
    }
}
