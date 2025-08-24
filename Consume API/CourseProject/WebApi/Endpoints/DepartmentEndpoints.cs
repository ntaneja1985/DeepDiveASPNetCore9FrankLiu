using WebApi.Filters;
using WebApi.Results;
using WebApi.Models;

namespace WebApi.Endpoints
{
    public static class DepartmentEndpoints
    {
        public static void MapDepartmentEndpoints(this WebApplication app)
        {

            app.MapGet("/departments/{filter?}", (string? filter, IDepartmentsRepository departmentsRepository) =>
            {
                var departments = departmentsRepository.GetDepartments(filter);

                return TypedResults.Ok(departments);
            });

            app.MapGet("/departments/{id:int}/exists", (int id, IDepartmentsRepository departmentsRepository) =>
            {
                var exists = departmentsRepository.DepartmentExists(id);
                return TypedResults.Ok(exists);
            });

            app.MapGet("/departments/{id:int}", (int id, IDepartmentsRepository departmentsRepository) =>
            {
                var department = departmentsRepository.GetDepartmentById(id);
                return TypedResults.Ok(department);
            }).AddEndpointFilter<EnsureDepartmentExistsFilter>();

            app.MapPost("/departments", (Department department, IDepartmentsRepository departmentsRepository) =>
            {
                departmentsRepository.AddDepartment(department);
                return TypedResults.Created($"/departments/{department.Id}", department);

            }).WithParameterValidation()
            .AddEndpointFilter<DepartmentCreateFilter>();

            app.MapPut("/departments/{id:int}", (int id, Department department, IDepartmentsRepository departmentsRepository) =>
            {
                departmentsRepository.UpdateDepartment(department);
                return TypedResults.NoContent();
            }).WithParameterValidation()
            .AddEndpointFilter<EnsureDepartmentExistsFilter>()
            .AddEndpointFilter<DepartmentUpdateFilter>();

            app.MapDelete("/departments/{id:int}", (int id, IDepartmentsRepository departmentsRepository) =>
            {
                var department = departmentsRepository.GetDepartmentById(id);
                departmentsRepository.DeleteDepartment(department);

                return TypedResults.Ok(department);
            }).AddEndpointFilter<EnsureDepartmentExistsFilter>();
        }
    }
}
