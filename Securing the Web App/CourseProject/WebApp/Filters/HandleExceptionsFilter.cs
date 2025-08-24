using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    public class HandleExceptionsFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            var error = new ProblemDetails
            {
                Title = "An error occurred",
                Detail = context.Exception.ToString(),
                Status = 500
            };

            context.Result = new ObjectResult(error)
            {
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }
    }
}
