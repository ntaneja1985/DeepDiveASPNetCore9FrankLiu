
namespace LearnRouting.RouteConstraints
{
    public class PositionConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // Check that routeKey exists in the values and is not null
            if (!values.ContainsKey(routeKey) || values[routeKey] == null)
                return false;
            // Convert value to string and compare case-insensitively
            var value = values[routeKey]?.ToString();
            return string.Equals(value, "manager", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(value, "developer", StringComparison.OrdinalIgnoreCase);
        }
    }
}
