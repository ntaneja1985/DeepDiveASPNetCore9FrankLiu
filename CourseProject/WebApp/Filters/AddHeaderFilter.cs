using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    public class AddHeaderFilter : Attribute, IResultFilter
    {
        public string? HeaderName { get; set; }
        public string? HeaderValue { get; set; }
        public void OnResultExecuted(ResultExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if(context.HttpContext.Response != null)
            {
                context.HttpContext.Response.Headers.Add(HeaderName ?? "X-Default-Header", HeaderValue ?? "DefaultValue");
            }
        }
    }
}
