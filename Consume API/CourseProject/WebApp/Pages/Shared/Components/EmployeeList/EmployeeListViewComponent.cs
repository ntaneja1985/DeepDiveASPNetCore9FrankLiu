using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Pages.Shared.Components.EmployeeList
{
    public class EmployeeListViewComponent : ViewComponent
    {
        private readonly IEmployeesRepository employeesRepository;

        public EmployeeListViewComponent(IEmployeesRepository employeesRepository)
        {
            this.employeesRepository = employeesRepository;
        }

        public IViewComponentResult Invoke(string? filter, int? departmentId)
        {
            return View(employeesRepository.GetEmployees(filter, departmentId));
        }
    }
}
