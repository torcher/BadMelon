using BadMelon.Data.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BadMelon.API.Helpers
{
    public class JwtAuthorizedFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Items["User"] == null)
                throw new UnauthorizedException();

            base.OnActionExecuting(context);
        }
    }
}