
using WebApi.Models;

namespace WebApi.Filters
{
    public class DepartmentCreateFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var department = context.GetArgument<Department>(0);

            if (department is null || department.Id < 0)
            {
                return Microsoft.AspNetCore.Http.Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        {"id", new[] { "Department is not provided or is not valid." } }
                    },
                statusCode: 400);
            }

            return await next(context);
        }
    }
}
