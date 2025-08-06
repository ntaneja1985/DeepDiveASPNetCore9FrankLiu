using System.Runtime.CompilerServices;

namespace LearnMinimalAPICodeOrganizationDependencyInjection.Endpoints
{
    public static class EmployeeEndpoints
    {
        public static void MapEmployeeEndpoints(this WebApplication app)
        {
            app.MapGet("/employees", async (HttpContext context) =>
            {
                await context.Response.WriteAsync("Get Employees");
            });
            app.MapPost("/employees", async (HttpContext context) =>
            {
                await context.Response.WriteAsync("Create an employee");
            });
            app.MapPut("/employees", async (HttpContext context) =>
            {
                await context.Response.WriteAsync("Update an employee");
            });
            app.MapDelete("/employees/{position}/{id}", async (HttpContext context, string position, int id) =>
            {
                await context.Response.WriteAsync($"Delete employee with ID {id} and position {position}");
            });
        }
    }
}
