using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    //public class MyActionFilter : Attribute, IActionFilter
    public class MyActionFilter: ActionFilterAttribute
    {
        //public void OnActionExecuting(ActionExecutingContext context)
        //{
        //    throw new NotImplementedException();
        //}
        //public void OnActionExecuted(ActionExecutedContext context)
        //{
        //    throw new NotImplementedException();
        //}

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        
    }
}
