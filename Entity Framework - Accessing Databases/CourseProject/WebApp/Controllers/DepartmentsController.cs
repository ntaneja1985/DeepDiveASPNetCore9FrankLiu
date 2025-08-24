using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.Filters;
using WebApp.Helpers;
using WebApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApp.Controllers
{
    [WriteToConsoleResourceFilter(Description = "Departments Controller")]
    public class DepartmentsController : Controller
    {        
        private readonly IDepartmentsApiRepository departmentsApiRepository;        

        public DepartmentsController(            
            IDepartmentsApiRepository departmentsApiRepository)
        {           
            this.departmentsApiRepository = departmentsApiRepository;           
        }

        [HttpGet]
        [WriteToConsoleResourceFilter(Description = "Index Method", Order = -1)]
        public IActionResult Index()
        { 
            return View();
        }

        //[Route("/department-list/{filter?}")]
        //public IActionResult SearchDepartments(string? filter)
        //{   
        //    var departments = departmentsRepository.GetDepartments(filter);
        //    return PartialView("_DepartmentList", departments);
        //}

        [Route("/department-list/{filter?}")]
        public IActionResult SearchDepartments(string? filter)
        {
            return ViewComponent("DepartmentList", new { filter });
        }

        [HttpGet]
        [EndpointExpiresFilter(ExpiryDate = "2028-01-18")]
        [EnsureDepartmentExistsFilter]
        public async Task<IActionResult> Details(int id)
        {
            var department = await departmentsApiRepository.GetDepartmentByIdAsync(id);

            return View(department);            
        }

        [HttpPost]
        [EnsureValidModelStateFilter]
        public async Task<IActionResult> Edit(Department department)
        {
            await departmentsApiRepository.UpdateDepartmentAsync(department);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {           
            return View(new Department());
        }

        [HttpPost]
        [EnsureValidModelStateFilter]
        public async Task<IActionResult> Create(Department department)
        {
            await departmentsApiRepository.AddDepartmentAsync(department);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [EnsureDepartmentExistsFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await departmentsApiRepository.GetDepartmentByIdAsync(id);

            await departmentsApiRepository.DeleteDepartmentAsync(department);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [HandleExceptionsFilter]
        public async Task<IActionResult> GetDepartments()
        {
            //throw new ApplicationException("Testing exception handling for web api endpoints.");
            var departments = await departmentsApiRepository.GetDepartmentsAsync();
            return Json(departments);
        }
        
    }
}
