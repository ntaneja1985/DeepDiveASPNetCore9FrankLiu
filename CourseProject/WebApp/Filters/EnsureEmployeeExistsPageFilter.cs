using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.Pages.Employees;

namespace WebApp.Filters
{
    public class EnsureEmployeeExistsPageFilter: Attribute,IPageFilter
    {
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
        }
        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (context.HandlerMethod?.MethodInfo.Name == "OnGet")
            {
                // If it's the OnGet handler, do nothing and exit the filter.
                return;
            }
            object? employeeId;
            if (context.HandlerArguments.ContainsKey("id")) {
                employeeId = context.HandlerArguments["id"];
            } else {
                employeeId = ((EditModel)context.HandlerInstance).EmployeeViewModel.Employee.Id;
            }

            var employeeRepository = context.HttpContext.RequestServices.GetRequiredService<IEmployeesRepository>();
            
            if (!employeeRepository.EmployeeExists((int)employeeId))
            {
                context.ModelState.AddModelError("id", "Employee not found.");
                var errors = ModelStateHelper.GetErrors(context.ModelState);
                context.Result = new RedirectToPageResult("/Error", new {  errors });
            }
        }
        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
        }
    }
    
}
