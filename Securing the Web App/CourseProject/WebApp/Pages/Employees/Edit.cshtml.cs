using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Filters;
using WebApp.Helpers;
using WebApp.Model;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Pages.Employees
{
    [EnsureValidModelStatePageFilter]
    [EnsureEmployeeExistsPageFilter]
    public class EditModel : PageModel
    {
        private readonly IEmployeesApiRepository employeesRepository;
        private readonly IDepartmentsApiRepository departmentsRepository;

        public EditModel(IEmployeesApiRepository employeesRepository, IDepartmentsApiRepository departmentsRepository)
        {
            this.employeesRepository = employeesRepository;
            this.departmentsRepository = departmentsRepository;
        }

        [BindProperty]
        public EmployeeViewModel? EmployeeViewModel { get; set; }

        public async Task OnGetAsync(int id)
        {
            this.EmployeeViewModel = new EmployeeViewModel();
            this.EmployeeViewModel.Employee = await employeesRepository.GetEmployeeByIdAsync(id);
            this.EmployeeViewModel.Departments = await departmentsRepository.GetDepartmentsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (EmployeeViewModel is not null && EmployeeViewModel.Employee != null)
            {
                await employeesRepository.UpdateEmployeeAsync(EmployeeViewModel.Employee);
            }

            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostDeleteEmplopyeeAsync(int id)
        {
            var employee = await employeesRepository.GetEmployeeByIdAsync(id);            

            await employeesRepository.DeleteEmployeeAsync(employee);

            return RedirectToPage("Index");
        }

    }
}
