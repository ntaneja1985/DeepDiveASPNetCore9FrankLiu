using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using WebApp.Helpers;

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
                var result = new RedirectToPageResult("/Error",new { errors});
            }
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
           
        }
    }
}
