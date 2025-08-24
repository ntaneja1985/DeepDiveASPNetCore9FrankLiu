using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Filters
{
    public class SpecialAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user is null || user.Identity is null || !user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
            }

            if (!user.HasClaim(c => c.Type == "CustomClaim" && c.Value == "CustomValue"))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
