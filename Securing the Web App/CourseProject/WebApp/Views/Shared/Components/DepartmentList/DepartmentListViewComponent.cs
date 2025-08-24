using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Views.Shared.Components.DepartmentList
{
    [ViewComponent]
    public class DepartmentListViewComponent : ViewComponent
    {
        private readonly IDepartmentsApiRepository departmentsRepository;

        public DepartmentListViewComponent(IDepartmentsApiRepository departmentsRepository)
        {
            this.departmentsRepository = departmentsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? filter)
        {
            var departments = await departmentsRepository.GetDepartmentsAsync(filter);
            return View(departments);
        }


    }
}
