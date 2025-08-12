using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Pages.Shared.Components.EmployeeList
{
    public class EmployeeListViewComponent(IEmployeesRepository employeesRepository) : ViewComponent
    {
        public IViewComponentResult Invoke(string? filter, int? departmentId)
        {
            return View(employeesRepository.GetEmployees(filter, departmentId));
        }
    }
}
