using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Filters
{
    public class EnsureValidModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            
            if (!context.ModelState.IsValid)
            {
                var result = new ViewResult { ViewName = "Error" };

                var controller = context.Controller as Controller;
                result.ViewData = controller.ViewData;
                result.ViewData.Model = ModelStateHelper.GetErrors(context.ModelState);

                context.Result = result;
            }
        }
    }
}
