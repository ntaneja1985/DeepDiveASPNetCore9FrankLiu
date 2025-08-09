using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Pages.Employees
{
    [BindProperties]
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        [FromBody]
        public int? Id { get; set; }

        [BindProperty]
        public InputModel? InputModel { get; set; }
        public void OnGetFirst()
        {
            var employeeId = Id;
        }

        public void OnGetSecond()
        {
            var employeeId = Id;
        }

        public void OnPost()
        {
        }

        public void OnPostSave()
        {
            // Logic to save the employee details
        }

        public void OnPostDelete()
        {
            // Logic to delete the employee
        }

        //public void OnPut()
        //{

        //}

        //public void OnDelete()
        //{
        //}
    }

    public class InputModel
    {
        public int? Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
