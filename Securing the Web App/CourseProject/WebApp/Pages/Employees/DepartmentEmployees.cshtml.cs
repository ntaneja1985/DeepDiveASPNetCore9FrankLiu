using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Pages.Employees
{
    public class DepartmentEmployeesModel : PageModel
    {
        private readonly IDepartmentsApiRepository departmentsRepository;

        public DepartmentEmployeesModel(IDepartmentsApiRepository departmentsRepository)
        {
            this.departmentsRepository = departmentsRepository;
        }

        public string? DepartmentName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? DepartmentId { get; set; }

        public async Task OnGetAsync()
        {
            if (DepartmentId.HasValue)
            {
                var department = await departmentsRepository.GetDepartmentByIdAsync(DepartmentId.Value);
                DepartmentName = department?.Name;
            }
        }
    }
}
