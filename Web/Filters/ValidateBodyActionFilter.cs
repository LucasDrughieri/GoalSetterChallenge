using Microsoft.AspNetCore.Mvc.Filters;
using Web.Utils;

namespace Web.Filters
{
    public class ValidateBodyActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                throw new AppException("The request body is invalid");
            }
        }
    }
}
