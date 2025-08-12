using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Models;

namespace WebApp.Filters
{
    public class EnsureDepartmentExistsFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            base.OnActionExecuting(context);

            var departmentId = (int)context.ActionArguments["id"];
            // 1. Manually get dependency from DI container
            var repo = context.HttpContext.RequestServices.GetRequiredService<IDepartmentsRepository>();
            if (!repo.DepartmentExists(departmentId))
            {
                // 3. Add a model error
                context.ModelState.AddModelError("id", "Department not found.");

                // 4. Manually create a ViewResult to short-circuit the request
                var viewResult = new ViewResult { ViewName = "Error" };
                // (More complex view data setup would be needed here as shown in the video)
                context.Result = viewResult;
            }

        }
    }
}
