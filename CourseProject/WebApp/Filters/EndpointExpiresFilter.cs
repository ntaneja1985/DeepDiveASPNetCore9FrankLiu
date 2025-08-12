using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    public class EndpointExpiresFilter : Attribute, IResourceFilter
    {
        public string? ExpiryDate { get; set; }
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if(DateTime.TryParse(ExpiryDate, out DateTime expiryDate))
            {
                if(DateTime.Now > expiryDate)
                {
                   context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult($"This endpoint has expired on {expiryDate}.");
                    return;
                }
            }
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            
        }
        
    }
}
