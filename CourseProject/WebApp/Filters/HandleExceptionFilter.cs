using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    public class HandleExceptionFilter: ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            var error = new ProblemDetails
            {
                Title = "An error occurred while processing your request.",
                Detail = context.Exception.Message,
                Status = 500, // Internal Server Error

            };

            context.Result = new ObjectResult(error)
            {
                StatusCode = 500
            };

            context.ExceptionHandled = true; // Mark the exception as handled
        }

    }
}
