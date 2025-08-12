
using WebApi.Models;

namespace WebApi.Filters
{
    //public class EmployeeUpdateFilter : IEndpointFilter
    //{
    //    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    //    {
    //        var id = context.GetArgument<int>(0);
    //        var employee = context.GetArgument<Employee>(1);

    //        if (id != employee.Id)
    //        {
    //            return Microsoft.AspNetCore.Http.Results.ValidationProblem(new Dictionary<string, string[]>
    //                {
    //                    {"id", new[] { "Employee id is not the same as id." } }
    //                },
    //            statusCode: 400);
    //        }

    //        return await next(context);
    //    }
    //}

    public class EmployeeUpdateFilter : IEndpointFilter
    {
        public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var id = context.GetArgument<int>(0);
            var employee = context.GetArgument<Employee>(1);
            if(id != employee.Id)
            {
                return new ValueTask<object?>(Microsoft.AspNetCore.Http.Results.ValidationProblem(
                    new Dictionary<string, string[]>
                    {
                        {"id", new[] { "Employee id is not the same as id." } }
                    },
                    statusCode: 400));
            }

            return next(context);
        }
    }
}
