
using WebApi.Models;

namespace WebApi.Filters
{
    public class DepartmentUpdateFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var id = context.GetArgument<int>(0);
            var department = context.GetArgument<Department>(1); 

            if (id != department.Id)
            {
                return Microsoft.AspNetCore.Http.Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        {"id", new[] { "Department id is not the same as id." } }
                    },
                statusCode: 400);
            }

            return await next(context);
        }
    }
}
