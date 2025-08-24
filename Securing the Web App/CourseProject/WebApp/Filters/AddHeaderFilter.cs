using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    public class AddHeaderFilter : Attribute, IResultFilter
    {
        public string? Name { get; set; }
        public string? Value { get; set; }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.HttpContext.Response != null &&
                this.Name is not null &&
                this.Value is not null &&
                context.HttpContext.Response.Headers is not null &&
                !context.HttpContext.Response.Headers.ContainsKey(this.Name))
            {
                context.HttpContext.Response.Headers.Add(Name, Value);
            }
        }
    }
}
