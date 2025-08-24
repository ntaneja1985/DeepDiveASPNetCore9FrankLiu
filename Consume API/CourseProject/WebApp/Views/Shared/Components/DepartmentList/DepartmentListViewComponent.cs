using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Views.Shared.Components.DepartmentList
{
    [ViewComponent]
    public class DepartmentListViewComponent : ViewComponent
    {
        private readonly IDepartmentsRepository departmentsRepository;

        public DepartmentListViewComponent(IDepartmentsRepository departmentsRepository)
        {
            this.departmentsRepository = departmentsRepository;
        }

        public IViewComponentResult Invoke(string? filter)
        {
            var departments = departmentsRepository.GetDepartments(filter);
            return View(departments);
        }


    }
}
