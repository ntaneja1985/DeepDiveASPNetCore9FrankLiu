using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    public class WriteToConsoleResourceFilter : Attribute, IResourceFilter, IOrderedFilter
    {
        public string Description { get; set; }

        public WriteToConsoleResourceFilter()
        {
            this.Description = "Global";
        }
        public int Order { get; set; }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine($"Executing {Description} - {context.ActionDescriptor.DisplayName}");
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine($"Executed {Description} - {context.ActionDescriptor.DisplayName}");
        }

        
    }
}
