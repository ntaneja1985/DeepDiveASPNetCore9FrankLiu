using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.Filters;
using WebApp.Helpers;
using WebApp.Model;
using WebApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApp.Controllers
{
    [WriteToConsoleResourceFilter(Description = "Departments Controller")]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentsApiRepository _departmentsApiRepository;

        public DepartmentsController(IDepartmentsApiRepository departmentsApiRepository)
        {
            _departmentsApiRepository = departmentsApiRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _departmentsApiRepository.GetDepartmentsAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _departmentsApiRepository.GetDepartmentByIdAsync(id);
            return View(model);
        }

        public IActionResult Create()
        {
            return View(new Department());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentsApiRepository.AddDepartmentAsync(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentsApiRepository.UpdateDepartmentAsync(department);
                return RedirectToAction(nameof(Index));
            }
            return View("Details", department);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentsApiRepository.DeleteDepartmentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
