using Dating.API.Services.Interfaces;
using Dating.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dating.API.Middleware
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultsContext = await next();
            if (context.HttpContext.User.Identity?.IsAuthenticated != true) return;

            var userId = resultsContext.HttpContext.User.GetUserId();
            var accountService = resultsContext.HttpContext.RequestServices.GetRequiredService<IAccountService>();

            await accountService.UpdateLastActivityDateAsync(userId);
        }
    }
}
