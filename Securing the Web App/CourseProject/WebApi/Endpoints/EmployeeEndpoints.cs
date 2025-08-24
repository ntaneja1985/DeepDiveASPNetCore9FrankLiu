using Asp.Versioning;
using WebApi.Filters;
using WebApi.Models;
using WebApi.Results;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Endpoints
{
    public static class EmployeeEndpoints
    {
        public static void MapEmployeeEndpoints(this WebApplication app)
        {
            app.MapGet("/", HtmlResult () =>
            {
                string html = "<h2>Welcome to our API</h2> Our API is used to learn ASP.NET CORE.";

                return new HtmlResult(html);
            });

            app.MapGet("/employees/search",            
                [Tags("Web API - Employees")]
                (string? filter, int? departmentId, IEmployeesRepository employeesRepository) =>
                {
                    if (string.IsNullOrWhiteSpace(filter)) filter = null;
                    if (departmentId == 0) departmentId = null;

                    var employees = employeesRepository.GetEmployees(filter, departmentId);

                    return TypedResults.Ok(employees);
                });

            app.MapGet("/employees", [Tags("Web API - Employees")] (IEmployeesRepository employeesRepository) =>
            {
                var employees = employeesRepository.GetEmployees();

                return TypedResults.Ok(employees);
            });

            app.MapGet("/employees/{id:int}/exists",
                [Tags("Web API - Employees")]
                (int id, IEmployeesRepository employeesRepository) =>
                {
                    var exists = employeesRepository.EmployeeExists(id);
                    return TypedResults.Ok(exists);
                });

            app.MapGet("/employees/{id:int}", [Tags("Web API - Employees")] (int id, IEmployeesRepository employeesRepository) =>
            {
                var employee = employeesRepository.GetEmployeeById(id);
                return TypedResults.Ok(employee);
            }).AddEndpointFilter<EnsureEmployeeExistsFilter>();

            app.MapPost("/employees", [Tags("Web API - Employees")] (Employee employee, IEmployeesRepository employeesRepository) =>
            {
                employeesRepository.AddEmployee(employee);
                return TypedResults.Created($"/employees/{employee.Id}", employee);

            }).WithParameterValidation()
            .AddEndpointFilter<EmployeeCreateFilter>();

            app.MapPut("/employees/{id:int}", [Tags("Web API - Employees")] (int id, Employee employee, IEmployeesRepository employeesRepository) =>
            {
                employeesRepository.UpdateEmployee(employee);
                return TypedResults.NoContent();                   
            }).WithParameterValidation()
            .AddEndpointFilter<EnsureEmployeeExistsFilter>()
            .AddEndpointFilter<EmployeeUpdateFilter>();            

            app.MapDelete("/employees/{id:int}", [Tags("Web API - Employees")] (int id, IEmployeesRepository employeesRepository) =>
            {
                var employee = employeesRepository.GetEmployeeById(id);
                employeesRepository.DeleteEmployee(employee);

                return TypedResults.Ok(employee);                    
            }).AddEndpointFilter<EnsureEmployeeExistsFilter>();
        }
    }
}
