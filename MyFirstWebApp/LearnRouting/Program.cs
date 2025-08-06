using LearnRouting.Models;
using LearnRouting.Results;
using LearnRouting.RouteConstraints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddRouting(options =>
//{
//    options.ConstraintMap.Add("position", typeof(PositionConstraint));
//});
builder.Services.AddProblemDetails();

var app = builder.Build();

if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();

    app.UseStatusCodePages();
}

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

//app.MapGet("/employees", () =>
//{
//    return Results.Ok(LearnRouting.Repository.EmployeeRepository.GetEmployees());
//});

app.MapGet("/", HtmlResult () =>
{
    string html = "<h2>Welcome to my API</h2>";
    return new HtmlResult(html);
});

app.MapGet("/employees", (int[] id) =>
{
    var employees = LearnRouting.Repository.EmployeeRepository.GetEmployees();
    var emps = employees.Where(x => id.Contains(x.Id)).ToList();
    return emps;
    //if (employee != null)
    //{
    //    employee.Name = param.Name;
    //    employee.Position = param.Position;
    //    return Results.Ok(employee);
    //}   
    //else
    //{
    //    return Results.NotFound("Employee not found.");
    //}
});

app.MapGet("/people",(Person? person) =>
{
    if (person == null)
    {
        return Results.BadRequest("Invalid person data.");
    }
    return Results.Ok($"Person ID: {person.Id}, Name: {person.Name}");
});

app.MapPost("/employees", (Employee employee) =>
{
    //var employee = await context.Request.ReadFromJsonAsync<Employee>();
    if (employee == null)
    {
        return Results.BadRequest("Invalid employee data.");
    }
    var validationResults = new List<ValidationResult>();
    var context = new ValidationContext(employee, null, null);
    if (!Validator.TryValidateObject(employee, context, validationResults, true))
    {
        // Collect errors and return as 400 Bad Request
        var errors = validationResults.Select(v => v.ErrorMessage).ToList();
        return Results.BadRequest(errors);
    }

    if (LearnRouting.Repository.EmployeeRepository.AddEmployee(employee))
    {
        return Results.Created($"/employees/{employee.Id}", employee);
    }
    else
    {
        return Results.Conflict("Employee with the same ID already exists.");
    }
});

app.MapPut("/employees", (Employee employee) =>
{
    //var employee = await context.Request.ReadFromJsonAsync<Employee>();
    if (employee == null)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { "id", new[] { "Employee is not provided or is invalid." } }
        });
    }
    if (LearnRouting.Repository.EmployeeRepository.UpdateEmployee(employee))
    {
        return TypedResults.Ok(employee);
    }
    else
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { "id", new[] { "Employee not found." } }
        });
    }
});

app.MapDelete("/employees/{id:int}", (int id) =>
{
    if (LearnRouting.Repository.EmployeeRepository.DeleteEmployee(id))
    {
        return TypedResults.Ok($"Employee with ID {id} deleted.");
    }
    else
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { "id", new[] { "Employee not found." } }
        });
    }
});

app.MapGet("/register/query", ([FromQuery] string email,
[FromQuery] string password,
[FromQuery(Name = "confirmPassword")] string confirmPassword) =>
{
    var reg = new Registration
    {
        Email = email,
        Password = password,
        ConfirmPassword = confirmPassword,
    };
    var validationResult = ValidateModel(reg);
    if (validationResult != null) return validationResult;

    // Registration success logic (no DB/persistence needed for this assignment)
    return Results.Ok("Registration info valid (from query string)!");
});

// 2. Registration via HTTP body (JSON)
app.MapPost("/register/body", ([FromBody] Registration reg) =>
{
    var validationResult = ValidateModel(reg);
    if (validationResult != null) return validationResult;

    // Registration success logic (no DB/persistence needed for this assignment)
    return Results.Ok("Registration info valid (from HTTP body)!");
});

// Helper for validation (because no WithParameterValidation extension here)
IResult ValidateModel(object model)
{
    var context = new ValidationContext(model);
    var results = new List<ValidationResult>();
    if (!Validator.TryValidateObject(model, context, results, true))
    {
        //return Results.BadRequest(results.Select(r => r.ErrorMessage).ToList());
        return Results.ValidationProblem(
            new Dictionary<string, string[]>
            {
                { "ValidationErrors", results.Select(r => r.ErrorMessage).ToArray() }
            });
    }
    return null;
}


app.Run();

class Person
{
    public int Id { get; set; }
    public string Name { get; set; }

    public static ValueTask<Person?> BindAsync(HttpContext context)
    {
        var id = context.Request.Query["id"].ToString();
        var nameString = context.Request.Query["name"].ToString();
        if (int.TryParse(id, out int parsedId))
        {
            return ValueTask.FromResult<Person?>(new Person { Id = parsedId, Name = nameString });
        }
        return ValueTask.FromResult<Person?>(null);
    }
}
class GetEmployeeParams
{
    [FromRoute]
    public int? Id { get; set; }

    [FromQuery]
    public string? Name { get; set; }

    [FromHeader]
    public string? Position { get; set; }
}


