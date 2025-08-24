
using WebApi.Models;

namespace WebApi.Filters
{
    public class EnsureEmployeeExistsFilter : IEndpointFilter
    {
        private readonly IEmployeesRepository employeesRepository;

        public EnsureEmployeeExistsFilter(IEmployeesRepository employeesRepository)
        {
            this.employeesRepository = employeesRepository;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var id = context.GetArgument<int>(0);

            if (!this.employeesRepository.EmployeeExists(id))
            {
                return Microsoft.AspNetCore.Http.Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        {"id", new[] { $"Employee with the id {id} doesn't exist." } }
                    },
                    statusCode: 404);
            }

            return await next(context);
        }
    }
}
