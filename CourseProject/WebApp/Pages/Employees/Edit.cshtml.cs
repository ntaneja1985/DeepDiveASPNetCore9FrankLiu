using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Filters;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Pages.Employees
{
    [EnsureValidModelStatePageFilter]
    [EnsureEmployeeExistsPageFilter]
    public class EditModel(IEmployeesRepository employeesRepository, IDepartmentsRepository departmentsRepository) : PageModel
    {
        [BindProperty]
        public EmployeeViewModel? EmployeeViewModel { get; set; }

        public void OnGet(int id)
        {
            this.EmployeeViewModel = new EmployeeViewModel();
            this.EmployeeViewModel.Employee = employeesRepository.GetEmployeeById(id);
            this.EmployeeViewModel.Departments = departmentsRepository.GetDepartments();
        }

        public IActionResult OnPost()
        {
            
            if (EmployeeViewModel is not null && EmployeeViewModel.Employee != null)
            {
                employeesRepository.UpdateEmployee(EmployeeViewModel.Employee);
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnPostDeleteEmplopyee(int id)
        {
            var employee = employeesRepository.GetEmployeeById(id);
            

            employeesRepository.DeleteEmployee(employee);

            return RedirectToPage("Index");
        }

    }
}
