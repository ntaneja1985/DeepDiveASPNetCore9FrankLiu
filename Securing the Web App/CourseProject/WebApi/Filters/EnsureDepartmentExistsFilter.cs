
using WebApi.Models;

namespace WebApi.Filters
{
    public class EnsureDepartmentExistsFilter : IEndpointFilter
    {
        private readonly IDepartmentsRepository departmentsRepository;

        public EnsureDepartmentExistsFilter(IDepartmentsRepository departmentsRepository)
        {
            this.departmentsRepository = departmentsRepository;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var id = context.GetArgument<int>(0);

            if (!this.departmentsRepository.DepartmentExists(id))
            {
                return Microsoft.AspNetCore.Http.Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        {"id", new[] { $"Department with the id {id} doesn't exist." } }
                    },
                    statusCode: 404);
            }

            return await next(context);
        }
    }
}
