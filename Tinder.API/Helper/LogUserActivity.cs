using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinder.API.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Tinder.API.Helper
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
             var resultContext = await next();

            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await repo.GetUser(userId);

            user.LastActive = DateTime.Now;
            await repo.SaveAll();
        }
    }
}
