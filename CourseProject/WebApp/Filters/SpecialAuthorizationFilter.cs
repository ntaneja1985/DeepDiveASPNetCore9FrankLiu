using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    public class SpecialAuthorizationFilter : Attribute,IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity == null || !context.HttpContext.User.Identity.IsAuthenticated)
            {
                // If not authenticated, short-circuit with a 401 Unauthorized result.
                context.Result = new UnauthorizedResult();
                return;
            }

            // 2. Check for AUTHORIZATION
            // Does the logged-in user have a specific claim?
            if (!context.HttpContext.User.HasClaim(c => c.Type == "CustomClaim" && c.Value == "CustomValue"))
            {
                // If they don't have the required claim, short-circuit with a 403 Forbidden result.
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
