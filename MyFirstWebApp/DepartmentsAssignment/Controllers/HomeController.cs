using Microsoft.AspNetCore.Mvc;

namespace DepartmentsAssignment.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("<h1>Welcome to Department Management</h1><a href='/departments'>Go to Departments</a>", "text/html");
        }
    }
}
