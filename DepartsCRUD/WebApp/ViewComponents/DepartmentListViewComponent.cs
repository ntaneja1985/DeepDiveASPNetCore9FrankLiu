using Microsoft.AspNetCore.Mvc;

namespace WebApp.ViewComponents
{
    [ViewComponent]
    public class DepartmentListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string? filter)
        {
            var departments = WebApp.Models.DepartmentsRepository.GetDepartments(filter);
            return View(departments);
        }
    }
}
