using BadMelon.Data.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace BadMelon.API.Helpers
{
    public class DTOValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                throw new ValidationException(
                    context.ModelState.ToDictionary(
                        m => m.Key,
                        m => string.Join(" ", m.Value.Errors.Select(e => e.ErrorMessage).ToArray())));
            }

            base.OnActionExecuting(context);
        }
    }
}