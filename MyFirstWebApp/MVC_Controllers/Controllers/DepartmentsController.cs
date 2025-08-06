using Microsoft.AspNetCore.Mvc;

namespace MVC_Controllers.Controllers
{
    [Controller]
    [Route("/api")]
    public class Departments
    {
        //Action Methods or endpoint handler

        [HttpGet("departments")]
        public string GetDepartments()
        {
            return "These are departments.";
        }

        [HttpGet("departments/{departmentId}")]
        public string GetDepartmentById(int departmentId)
        {
            return $"Department Info: {departmentId}";
        }

        [NonAction]
        public string GetDepartmentById(string name)
        {
            return $"Department Info: {name}";
        }
    }
}
