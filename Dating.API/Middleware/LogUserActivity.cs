using Dating.API.Extensions;
using Dating.API.Services.Interfaces;
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
            var userService = resultsContext.HttpContext.RequestServices.GetRequiredService<IUsersService>();

            await userService.UpdateLastActivityDateAsync(userId);
        }
    }
}
