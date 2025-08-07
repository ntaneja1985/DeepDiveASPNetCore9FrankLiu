using DepartmentsAssignment.Models;
using DepartmentsAssignment.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentsAssignment.Controllers
{
    public class DepartmentsController : Controller
    {
        // 1. List Departments
        [HttpGet]
        public IActionResult Index()
        {
            var departments = DepartmentsRepository.GetDepartments();
            var html = @"<h1>Departments</h1>
            <ul>" + string.Join("\n", departments.Select(x => $"<li><a href='/departments/details/{x.Id}'>{x.Name}</a> ({x.Description})</li>")) + @"</ul> <a href='/departments/create'>Add Department</a>"; return Content(html, "text/html");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var dept = DepartmentsRepository.GetDepartmentById(id);
            if (dept == null)
                return Content("<h2 style='color:red'>Department not found</h2><a href='/departments'>Back to list</a>", "text/html");

            var html = $@"<h1>{dept.Name} Details</h1> <p>Description: {dept.Description}</p> <form method='post' action='/departments/edit'> <input type='hidden' name='Id' value='{dept.Id}' /> Name: <input name='Name' value='{dept.Name}' /><br/> Description: <input name='Description' value='{dept.Description}' /><br/> <button type='submit'>Update</button> </form> <form method='post' action='/departments/delete/{dept.Id}' style='display:inline'> <button type='submit' style='background:red;color:white'>Delete</button> </form> <a href='/departments'>Cancel</a>";
            return Content(html, "text/html");
        }

        [HttpPost]
        public IActionResult Edit([FromForm] Department department)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(department.Name))
                return Content(GetErrorsHtml() + "<a href='/departments'>Back</a>", "text/html");

            DepartmentsRepository.UpdateDepartment(department);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var html = @"<h1>Add Department</h1> <form method='post' action='/departments/create'> Name: <input name='Name' /><br/> Description: <input name='Description' /><br/> <button type='submit'>Add</button> <a href='/departments'>Cancel</a> </form>";
            return Content(html, "text/html");
        }

        // 5. Create (POST)
        [HttpPost]
        public IActionResult Create([FromForm] Department department)
        {
            if (string.IsNullOrWhiteSpace(department.Name))
                return Content(GetErrorsHtml() + "<a href='/departments/create'>Try Again</a>", "text/html");
            DepartmentsRepository.AddDepartment(department);
            return RedirectToAction("Index");
        }

        // 6. Delete (POST)
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dept = DepartmentsRepository.GetDepartmentById(id);
            if (dept == null)
            {
                ModelState.AddModelError("id", $"Department with id {id} does not exist.");
                return Content(GetErrorsHtml() + "<a href='/departments'>Back</a>", "text/html");
            }
            DepartmentsRepository.DeleteDepartment(dept);
            return RedirectToAction("Index");
        }

        // Error message HTML generator
        private string GetErrorsHtml()
        {
            var errors = new List<string>();
            foreach (var entry in ModelState.Values)
                foreach (var error in entry.Errors)
                    errors.Add(error.ErrorMessage);

            if (errors.Count == 0)
                errors.Add("Name is required.");

            return "<ul style='color:red'>" + string.Join("", errors.Select(e => $"<li>{e}</li>")) + "</ul>";
        }

    }
}
