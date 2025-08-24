using WebApi.Filters;
using WebApi.Results;
using WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning.Builder;
using Asp.Versioning;

namespace WebApi.Endpoints
{
    public static class DepartmentEndpoints
    {
        public static void MapDepartmentEndpoints(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            app.MapGet("/departments/search/{filter?}",
            [EndpointSummary("Get Departments v1")]
            [EndpointDescription("Get all the departments or get departments by filters.")]
            [Tags("Web API - Departments")]
            (string? filter, IDepartmentsRepository departmentsRepository) =>
                {
                    Console.WriteLine("Version: 1");

                    var departments = departmentsRepository.GetDepartments(filter);

                    return TypedResults.Ok(departments);
                })
                .WithApiVersionSet(apiVersionSet)
                .MapToApiVersion(new ApiVersion(1, 0))
                .WithGroupName("v1");

            app.MapGet("/departments/search/{filter?}",            
            [EndpointSummary("Get Departments v2")]
            [EndpointDescription("Get all the departments or get departments by filters.")]
            [Tags("Web API - Departments")]
            (string? filter, IDepartmentsRepository departmentsRepository) =>
                {
                    Console.WriteLine("Version: 2");

                    var departments = departmentsRepository.GetDepartments(filter);

                    return TypedResults.Ok(departments);
                })
                .WithApiVersionSet(apiVersionSet)
                .MapToApiVersion(new ApiVersion(2, 0))
                .WithGroupName("v2");

            app.MapGet("/departments/{id:int}/exists",
                [Tags("Web API - Departments")]
            (int id, IDepartmentsRepository departmentsRepository) =>
            {
                var exists = departmentsRepository.DepartmentExists(id);
                return TypedResults.Ok(exists);
            });

            app.MapGet("/departments/{id:int}", [Tags("Web API - Departments")] (int id, IDepartmentsRepository departmentsRepository) =>
            {
                var department = departmentsRepository.GetDepartmentById(id);
                return TypedResults.Ok(department);
            }).AddEndpointFilter<EnsureDepartmentExistsFilter>();

            app.MapPost("/departments", [Tags("Web API - Departments")] (Department department, IDepartmentsRepository departmentsRepository) =>
            {
                departmentsRepository.AddDepartment(department);
                return TypedResults.Created($"/departments/{department.Id}", department);

            }).WithParameterValidation()
            .AddEndpointFilter<DepartmentCreateFilter>();

            app.MapPut("/departments/{id:int}", 
                [Tags("Web API - Departments")]
                [ProducesResponseType(404)]
                (int id, Department department, IDepartmentsRepository departmentsRepository) =>
            {
                departmentsRepository.UpdateDepartment(department);
                return TypedResults.NoContent();
            }).WithParameterValidation()
            .AddEndpointFilter<EnsureDepartmentExistsFilter>()
            .AddEndpointFilter<DepartmentUpdateFilter>();

            app.MapDelete("/departments/{id:int}", 
                [Tags("Web API - Departments")]
                [ProducesResponseType(404)]
                (int id, IDepartmentsRepository departmentsRepository) =>
            {
                var department = departmentsRepository.GetDepartmentById(id);
                departmentsRepository.DeleteDepartment(department);

                return TypedResults.Ok(department);
            }).AddEndpointFilter<EnsureDepartmentExistsFilter>();
        }
    }
}
