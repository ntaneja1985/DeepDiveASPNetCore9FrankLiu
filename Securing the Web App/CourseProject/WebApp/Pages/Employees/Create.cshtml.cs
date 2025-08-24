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
    public class CreateModel : PageModel
    {
        private readonly IDepartmentsApiRepository departmentsRepository;
        private readonly IEmployeesApiRepository employeesRepository;

        public CreateModel(
            IDepartmentsApiRepository departmentsRepository,
            IEmployeesApiRepository employeesRepository)
        {
            this.departmentsRepository = departmentsRepository;
            this.employeesRepository = employeesRepository;
        }

        [BindProperty]
        public EmployeeViewModel? EmployeeViewModel { get; set; }

        public async Task OnGetAsync()
        {
            this.EmployeeViewModel = new EmployeeViewModel();
            this.EmployeeViewModel.Employee = new Employee();
            this.EmployeeViewModel.Departments = await departmentsRepository.GetDepartmentsAsync();
        }

        public async Task<IActionResult> OnPostAsync() 
        {   
            if (this.EmployeeViewModel is not null && this.EmployeeViewModel.Employee is not null)
            {
                await employeesRepository.AddEmployeeAsync(this.EmployeeViewModel.Employee);
            }

            return RedirectToPage("Index");
        }
    }
}
