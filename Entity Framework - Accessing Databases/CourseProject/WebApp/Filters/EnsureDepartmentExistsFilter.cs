using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Filters
{
    public class EnsureDepartmentExistsFilter: ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var departmentId = (int)context.ActionArguments["id"];

            var departmentsRepository = context.HttpContext.RequestServices.GetService<IDepartmentsApiRepository>();

            if (!await departmentsRepository.DepartmentExistsAsync(departmentId))
            {
                context.ModelState.AddModelError("id", "Department not found.");

                var result = new ViewResult { ViewName = "Error" };

                var controller = context.Controller as Controller;
                result.ViewData = controller.ViewData;
                result.ViewData.Model = ModelStateHelper.GetErrors(context.ModelState);

                context.Result = result;
                
            }

            await base.OnActionExecutionAsync(context, next);
        }

        
        
    }
}
