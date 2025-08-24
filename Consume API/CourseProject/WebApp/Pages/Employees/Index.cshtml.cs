using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Filters;
using WebApp.Models;

namespace WebApp.Pages.Employees
{    
    public class IndexModel : PageModel
    {        
        public List<Employee>? Employees { get; set; }

        public void OnGet()
        {
            //this.Employees = EmployeesRepository.GetEmployees();
        }        

        public IActionResult OnGetSearchEmployeeResult(string? filter)
        {
            return ViewComponent("EmployeeList", new { filter });
        }
    }
}
