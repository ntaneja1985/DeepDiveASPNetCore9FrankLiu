using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    public class EndpointExpiresFilter : Attribute, IResourceFilter
    {
        public string? ExpiryDate { get; set; }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (DateTime.TryParse(ExpiryDate, out DateTime expiryDate))
            {
                if (DateTime.Now > expiryDate) 
                {
                    context.Result = new BadRequestResult();
                }
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {            
        }        
    }
}
