using Microsoft.AspNetCore.Mvc;

namespace MVC_Controllers.Controllers
{
    [Controller]
    [Route("/api")]
    public class DepartmentsController: Controller
    {
        //Action Methods or endpoint handler

        [HttpGet("departments")]
        public string GetDepartments()
        {
            return "These are departments.";
        }

        [HttpPost]
        public object CreateDepartment([FromBody] string departmentName)
        {
            foreach(var value in ModelState.Values)
            {
                foreach(var error in value.Errors)
                {
                    // Log the error or handle it as needed
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            // In a real application, you would typically save the department to a database
            return new { Message = $"Department '{departmentName}' created successfully." };
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
