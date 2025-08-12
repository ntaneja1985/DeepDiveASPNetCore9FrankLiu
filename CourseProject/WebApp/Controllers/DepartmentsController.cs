using Microsoft.AspNetCore.Mvc;
using WebApp.Filters;
using WebApp.Helpers;
using WebApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApp.Controllers
{
    [WriteToConsoleResourceFilter(Description ="Inside controller filter")]
    public class DepartmentsController(IDepartmentsRepository departmentsRepository) : Controller
    {
        [HttpGet]
        //[TypeFilter(typeof(WriteToConsoleResourceFilter))]
        [WriteToConsoleResourceFilter(Description ="Inside action method", Order = -1)]
        public IActionResult Index()
        { 
            return View();
        }

        //[Route("/department-list/{filter?}")]
        //public IActionResult SearchDepartments(string? filter)
        //{   
        //    var departments = DepartmentsRepository.GetDepartments(filter);
        //    return PartialView("_DepartmentList", departments);
        //}

        [Route("/department-list/{filter?}")]
        public IActionResult SearchDepartments(string? filter)
        {
            return ViewComponent("DepartmentList", new { filter });
        }

        [HttpGet]
        //[TypeFilter(typeof(WriteToConsoleResourceFilter))]
        //[WriteToConsoleResourceFilter]
        [EndpointExpiresFilter(ExpiryDate = "2025-12-31")]
        [EnsureDepartmentExistsFilter]
        public IActionResult Details(int id)
        {
            var department = departmentsRepository.GetDepartmentById(id);
            

            return View(department);
            
        }

        [HttpPost]
        [EnsureValidModelStateFilter]
        public IActionResult Edit(Department department)
        {

            departmentsRepository.UpdateDepartment(department);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {           
            return View(new Department());
        }

        [HttpPost]
        [EnsureValidModelStateFilter]
        public IActionResult Create(Department department)
        {
            departmentsRepository.AddDepartment(department);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [EnsureDepartmentExistsFilter]
        public IActionResult Delete(int id)
        {
            var department = departmentsRepository.GetDepartmentById(id);
            

            departmentsRepository.DeleteDepartment(department);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [HandleExceptionFilter]
        public IActionResult GetDepartments()
        {
            throw new ApplicationException("Testing exception handling");
            //var departments = departmentsRepository.GetDepartments();
            //return Json(departments);
        }

        
        
    }
}
