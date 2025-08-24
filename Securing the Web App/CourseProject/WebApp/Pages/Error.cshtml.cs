using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class ErrorModel : PageModel
    {
        public List<string>? Errors  { get; set; }

        public void OnGet(List<string> errors)
        {
            Errors = errors ?? new List<string>();
        }
    }
}
