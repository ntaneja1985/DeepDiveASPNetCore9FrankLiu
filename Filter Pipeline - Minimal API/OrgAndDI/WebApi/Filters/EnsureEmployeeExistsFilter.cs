
using WebApi.Models;

namespace WebApi.Filters
{
    public class EnsureEmployeeExistsFilter(IEmployeesRepository employeesRepository) : IEndpointFilter
    {
        
        public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var id = context.GetArgument<int>(0);
            var employeeExists = employeesRepository.EmployeeExists(id);
            if (!employeeExists)
            {
                return new ValueTask<object?>(Microsoft.AspNetCore.Http.Results.ValidationProblem(
                    new Dictionary<string, string[]>
                    {
                        {"id", new[] { $"Employee doesnot exist for this ID {id} " } }
                    },
                    statusCode: 400));
            }

            return next(context);
        }
    }
}
