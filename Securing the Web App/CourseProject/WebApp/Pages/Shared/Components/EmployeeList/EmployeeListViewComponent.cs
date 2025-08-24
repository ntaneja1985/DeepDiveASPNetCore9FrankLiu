using Microsoft.AspNetCore.Mvc;
using WebApp.Model;
using WebApp.Models;

namespace WebApp.Pages.Shared.Components.EmployeeList
{
    public class EmployeeListViewComponent : ViewComponent
    {
        private readonly IEmployeesApiRepository employeesRepository;

        public EmployeeListViewComponent(IEmployeesApiRepository employeesRepository)
        {
            this.employeesRepository = employeesRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? filter, int? departmentId)
        {
            var employees = await employeesRepository.GetEmployeesAsync(filter, departmentId);

            return View(employees);
        }
    }
}
