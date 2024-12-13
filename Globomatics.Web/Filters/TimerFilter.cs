using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Globomatics.Web.Filters
{
    public class TimerFilter : IAsyncActionFilter
    {
        private readonly ILogger<TimerFilter> logger;

        public TimerFilter(ILogger<TimerFilter> logger)
        {
            this.logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch =new Stopwatch();
            stopwatch.Start();

            logger.LogInformation($"Starting execution of {context.ActionDescriptor.DisplayName}");

            await next();

            stopwatch.Stop();
            logger.LogInformation
                ($"Execution completed for {context.ActionDescriptor.DisplayName} in {stopwatch.ElapsedMilliseconds}");

        }
    }
}
