using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Filters
{
    public class EnsureValidModelStatePageFilter : Attribute, IPageFilter
    {
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = ModelStateHelper.GetErrors(context.ModelState);                
                context.Result = new RedirectToPageResult("/Error", new { errors });
            }
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            
        }
    }
}
