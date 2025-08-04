//Create a Kestrel server
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

//Configure the Kestrel server options
builder.WebHost.ConfigureKestrel(options =>
{
    // Set the maximum request body size to 10 MB
    options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
});

// Creates an instance of the web application
var app = builder.Build();

// Configure the HTTP request middleware pipeline

//app.MapGet("/", () => "Hello World!");
app.Run(async (HttpContext context) =>
{


    //foreach(var key in context.Request.Query.Keys)
    //{
    //    await context.Response.WriteAsync($"{key} : {context.Request.Query[key]}\r\n");
    //}

    //await context.Response.WriteAsync(context.Request.QueryString.ToString());

    if (context.Request.Path.StartsWithSegments("/"))
    {
        if (context.Request.Method == "GET")
        {

            // Set the response content type to text/plain
            //context.Response.ContentType = "text/plain";
            context.Response.Headers["Content-Type"] = "text/html";

            // Write a simple response
            await context.Response.WriteAsync($"The method is : {context.Request.Method}<br/>");
            await context.Response.WriteAsync($"The url is : {context.Request.Path}<br/>");
            await context.Response.WriteAsync($"<ul>");
            foreach (var key in context.Request.Headers.Keys)
            {
                await context.Response.WriteAsync($"<li><b>{key}</b> : {context.Request.Headers[key]}</li>");
            }
            await context.Response.WriteAsync($"</ul>");

        }

    }
    else if (context.Request.Path.StartsWithSegments("/employees"))
    {
        if (context.Request.Method == "GET")
        {
            if (context.Request.Query.ContainsKey("id"))
            {
                var id = context.Request.Query["id"];
                var employee = EmployeeRepository.GetById(int.Parse(id));
                if (employee != null)
                {
                    context.Response.Headers.ContentType = "text/html";
                    await context.Response.WriteAsync("<b>Employee Details</b> <br/>");
                    await context.Response.WriteAsync("<ul>");
                    await context.Response.WriteAsync($"<li>{employee.Name} - {employee.Position}</li>");
                    await context.Response.WriteAsync($"<li>{employee.Name} - {employee.Salary}</li>");
                    await context.Response.WriteAsync("</ul>");
                }
            }
            else
            {

                EmployeeRepository.Add(new Employee(1, "Alice Johnson", "Manager", 85000));
                EmployeeRepository.Add(new Employee(2, "Bob Smith", "Software Engineer", 70000));
                EmployeeRepository.Add(new Employee(3, "Carol Martinez", "QA Analyst", 65000));
                EmployeeRepository.Add(new Employee(4, "David Lee", "DevOps Engineer", 78000));
                EmployeeRepository.Add(new Employee(5, "Eva Brown", "HR Specialist", 62000));
                var employees = EmployeeRepository.GetAll();
                foreach (var emp in employees)
                {
                    await context.Response.WriteAsync($"{emp.Name} : {emp.Position}\r\n");
                }
            }
        }
        else if (context.Request.Method == "POST")
        {
            if (context.Request.Path.StartsWithSegments("/employees"))
            {
                //Get the information in the Http Body
                using var reader = new StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();
                var employee = JsonSerializer.Deserialize<Employee>(body);
                EmployeeRepository.Add(employee);
                context.Response.StatusCode = 201; // Created

            }
        }
        else if (context.Request.Method == "PUT")
        {

            //Get the information in the Http Body
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            var employee = JsonSerializer.Deserialize<Employee>(body);
            var result = EmployeeRepository.Update(employee);
            context.Response.StatusCode = result ? 204 : 404; // No Content or Not Found
            if (result)
            {
                await context.Response.WriteAsync("Employee updated");
            }
            else
            {
                await context.Response.WriteAsync("No employee found to update");
            }

        }
        else if (context.Request.Method == "DELETE")
        {
            if (context.Request.Query.ContainsKey("id"))
            {
                var id = context.Request.Query["id"];
                if (int.TryParse(id, out int employeeId))
                {
                    if (context.Request.Headers["Authorization"] == "frank")
                    {
                        var result = EmployeeRepository.Remove(employeeId);
                        if (result)
                        {
                            await context.Response.WriteAsync("Employee deleted");
                        }
                        else
                        {
                            await context.Response.WriteAsync("No employee found to delete");
                        }
                    }
                    else
                    {
                        await context.Response.WriteAsync("You are not authorized");
                    }

                }

            }

        }
    }
});

// Runs the web application and starts the Kestrel server
app.Run();



// Employee class
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public decimal Salary { get; set; }

    public Employee(int id, string name, string position, decimal salary)
    {
        Id = id;
        Name = name;
        Position = position;
        Salary = salary;
    }
}

// Static EmployeeRepository class
public static class EmployeeRepository
{
    // Internal store for employees
    private static List<Employee> employees = new List<Employee>();

    // Add a new employee
    public static void Add(Employee employee)
    {
        employees.Add(employee);
    }

    // Get an employee by Id
    public static Employee GetById(int id)
    {
        return employees.FirstOrDefault(e => e.Id == id);
    }

    // Get all employees
    public static List<Employee> GetAll()
    {
        return new List<Employee>(employees);
    }

    // Remove employee by Id
    public static bool Remove(int id)
    {
        var employee = GetById(id);
        if (employee == null)
            return false;
        return employees.Remove(employee);
    }

    // Update an employee
    public static bool Update(Employee updatedEmployee)
    {
        var existingEmployee = GetById(updatedEmployee.Id);
        if (existingEmployee == null)
            return false;

        existingEmployee.Name = updatedEmployee.Name;
        existingEmployee.Position = updatedEmployee.Position;
        existingEmployee.Salary = updatedEmployee.Salary;
        return true;
    }
}
