using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Pages.Employees
{
    public class DepartmentEmployeesModel(IDepartmentsRepository departmentsRepository) : PageModel
    {
        public string? DepartmentName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? DepartmentId { get; set; }

        public void OnGet()
        {
            if (DepartmentId.HasValue)
            {
                var department = departmentsRepository.GetDepartmentById(DepartmentId.Value);
                DepartmentName = department?.Name;
            }
        }
    }
}
