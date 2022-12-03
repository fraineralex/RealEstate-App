using Microsoft.AspNetCore.Mvc.Filters;
using RealEstateApp.Presentation.WebApp.Controllers;

namespace RealEstateApp.Presentation.WebApp.Middlewares
{
    public class LoginAuthorize : IAsyncActionFilter
    {
        private readonly ValidateUserSession _userSession;

        public LoginAuthorize(ValidateUserSession userSession)
        {
            _userSession = userSession;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_userSession.HasUser())
            {
                var controller = (HomeController)context.Controller;
                context.Result = controller.RedirectToAction("Index", "Post");
            }

            else
            {
                await next();
            }
        }
    }
}
