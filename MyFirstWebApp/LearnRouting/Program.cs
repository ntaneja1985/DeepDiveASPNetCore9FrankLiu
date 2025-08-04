using LearnRouting.Models;
using LearnRouting.RouteConstraints;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Drawing;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddRouting(options =>
//{
//    options.ConstraintMap.Add("position", typeof(PositionConstraint));
//});
var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    // Before routing middleware runs
//    Console.WriteLine($"Before routing: Endpoint is {(context.GetEndpoint()?.DisplayName ?? "(null)")}");
//    await next();
//    // After routing middleware runs
//    Console.WriteLine($"After routing: Endpoint is {(context.GetEndpoint()?.DisplayName ?? "(null)")}");
//});

app.UseRouting();


//app.MapGet("/employees", async (HttpContext context) =>
//{
//    await context.Response.WriteAsync("Get Employees");
//});

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapGet("/employees", async (HttpContext context) =>
//    {
//        await context.Response.WriteAsync("Get Employees");
//    });

//    endpoints.MapPost("/employees", async (HttpContext context) =>
//    {
//        await context.Response.WriteAsync("Create an employee");
//    });

//    endpoints.MapPut("/employees", async (HttpContext context) =>
//    {
//        await context.Response.WriteAsync("Update an employee");
//    });

//    endpoints.MapDelete("/employees/{position}/{id}", async (HttpContext context) =>
//    {
//        await context.Response.WriteAsync("Delete an employee");
//    });

//    endpoints.MapGet("/employees/{id}", async (HttpContext context) =>
//    {
//        await context.Response.WriteAsync($"Get employee: {context.Request.RouteValues["id"]}");
//    });

//    endpoints.MapGet("/employees/{name}", async (HttpContext context) =>
//    {
//        await context.Response.WriteAsync($"Get employee: {context.Request.RouteValues["name"]}");
//    });
//});

//app.MapGet("/categories/{category=shirts}/{size=medium}/{id?}", async context =>
//{
//    var category = context.Request.RouteValues["category"];
//    var size = context.Request.RouteValues["size"];
//    var id = context.Request.RouteValues["id"];
//    await context.Response.WriteAsync($"Category: {category}, Size: {size}, ID: {id}");
//});

//app.MapGet("/employees/positions/{position:position}", async (HttpContext context) =>
//{
//    await context.Response.WriteAsync($"Position: {context.Request.RouteValues["position"]}");
//});

app.MapGet("/employees", () =>
{
    return Results.Ok(LearnRouting.Repository.EmployeeRepository.GetEmployees());
});

app.MapGet("/employees/{id:int}", (int id) =>
{
    var employee = LearnRouting.Repository.EmployeeRepository.GetEmployee(id);
    if (employee != null)
    {
        return Results.Ok(employee);
    }
    else
    {
        return Results.NotFound("Employee not found.");
    }
});

app.MapPost("/employees", async (HttpContext context) =>
{
    var employee = await context.Request.ReadFromJsonAsync<Employee>();
    if(employee == null)
    {
        return Results.BadRequest("Invalid employee data.");
    }
    if (LearnRouting.Repository.EmployeeRepository.AddEmployee(employee))
    {
        return Results.Created($"/employees/{employee.Id}", employee);
    } else
    {
        return Results.Conflict("Employee with the same ID already exists.");
    }
});

app.MapPut("/employees", async (HttpContext context) =>
{
    var employee = await context.Request.ReadFromJsonAsync<Employee>();
    if (employee == null)
    {
        return Results.BadRequest("Invalid employee data.");
    }
    if (LearnRouting.Repository.EmployeeRepository.UpdateEmployee(employee))
    {
        return Results.Ok(employee);
    }
    else
    {
        return Results.NotFound("Employee not found.");
    }
});

app.MapDelete("/employees/{id:int}", (int id) =>
{
    if (LearnRouting.Repository.EmployeeRepository.DeleteEmployee(id))
    {
        return Results.Ok($"Employee with ID {id} deleted.");
    }
    else
    {
        return Results.NotFound("Employee not found.");
    }
});


app.Run();
