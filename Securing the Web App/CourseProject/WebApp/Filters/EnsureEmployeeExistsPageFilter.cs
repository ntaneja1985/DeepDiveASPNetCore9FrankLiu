using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Helpers;
using WebApp.Model;
using WebApp.Models;
using WebApp.Pages.Employees;

namespace WebApp.Filters
{
    public class EnsureEmployeeExistsPageFilter : Attribute, IAsyncPageFilter
    {
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (context.HandlerMethod is not null && !context.HandlerMethod.MethodInfo.Name.StartsWith("OnGet"))
            {
                var employeeRespository = context.HttpContext.RequestServices.GetService<IEmployeesApiRepository>();

                object? employeeId;
                if (context.HandlerArguments.ContainsKey("id"))
                    employeeId = context.HandlerArguments["id"];
                else
                    employeeId = ((EditModel)context.HandlerInstance).EmployeeViewModel.Employee.Id;


                if (!await employeeRespository.EmployeeExistsAsync((int)employeeId))
                {
                    context.ModelState.AddModelError("id", "Employee not found.");
                    var errors = ModelStateHelper.GetErrors(context.ModelState);

                    context.Result = new RedirectToPageResult("/Error", new { errors });
                }
            }            

            await next();
        }        

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }
    }
}
