using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Views.Shared.Components.DepartmentList
{
    [ViewComponent]
    public class DepartmentListViewComponent(IDepartmentsRepository departmentsRepository) : ViewComponent
    {
        public IViewComponentResult Invoke(string? filter)
        {
            var departments = departmentsRepository.GetDepartments(filter);
            return View(departments);
        }


    }
}
