# Dependency Injection in MVC and Razor Pages

* **The Goal: Moving from Static to Injected Repositories:** The main objective is to refactor the application from using `static` repositories to using instance-based repositories managed by a **Dependency Injection (DI)** container. This is a standard practice for building scalable and testable applications.
* **What is Dependency Injection?** DI is a design pattern where a class receives its dependencies (other objects it needs to work) from an external source rather than creating them itself. This promotes **loose coupling**.
* **Constructor Injection vs. Method Injection:**
    * **Method Injection:** Used in Minimal APIs. The dependency is passed as a parameter directly into the endpoint handler method. The dependency's scope is limited to that method call.
    * **Constructor Injection:** The standard pattern for MVC controllers and Razor PageModels. Dependencies are injected into the class's **constructor** and are typically stored in a private field, making them available to all methods within that class.
* **Two Critical Preparatory Steps:** Before you can use constructor injection, you must:

1. **Remove the `static` keyword:** A DI container cannot manage static classes because they cannot be instantiated. You must convert them to regular instance classes.
2. **Extract an Interface:** Create an interface for your class (e.g., `IEmployeeRepository` for `EmployeeRepository`). Your application's components (like controllers) will depend on the *interface*, not the concrete class. This is the key to achieving loose coupling.


### Illustrations with Code Examples and a Diagram

#### **Diagram: Constructor Injection vs. Method Injection**

This diagram shows the fundamental difference in where the dependency is supplied.

```
+-------------------------------------------------+
| Method Injection (e.g., Minimal API)            |
|-------------------------------------------------+
| app.MapGet("/employees/{id}",                   |
|   (int id, IEmployeeRepository repo) =>         |
|   {                                             |
|     // 'repo' is only available inside this method |
|     return repo.GetById(id);                   |
|   });                                           |
+-------------------------------------------------+

+-------------------------------------------------+
| Constructor Injection (e.g., MVC Controller)    |
|-------------------------------------------------+
| public class EmployeesController : Controller   |
| {                                               |
|   private readonly IEmployeeRepository _repo;   |
|                                                 |
|   // Dependency is injected here into the constructor |
|   public EmployeesController(IEmployeeRepository repo) |
|   {                                             |
|     _repo = repo; // Stored for use by the whole class |
|   }                                             |
|                                                 |
|   public IActionResult Index()                 |
|   {                                             |
|     // '_repo' is available everywhere in the class |
|     var employees = _repo.GetAll();             |
|     return View(employees);                     |
|   }                                             |
| }                                               |
+-------------------------------------------------+
```


#### **Code Example: Refactoring the Repository**

This example shows the "before" and "after" of the refactoring steps described in the transcript.

**1. Before (Static Repository)**
This class cannot be used with dependency injection. All methods and fields are `static`.

```csharp
public static class EmployeeRepository
{
    private static List<Employee> _employees = ...;

    public static List<Employee> GetEmployees()
    {
        return _employees;
    }
    // ... other static methods
}
```

**2. After (Instance Class with an Interface)**
The class is no longer static, and an interface has been extracted. This is ready for DI.

**The New Interface (`IEmployeeRepository.cs`):**

```csharp
// The contract that other classes will depend on
public interface IEmployeeRepository
{
    List<Employee> GetEmployees();
    // ... other method signatures
}
```

**The Refactored Class (`EmployeeRepository.cs`):**

```csharp
// Implements the interface, no more 'static' keyword
public class EmployeeRepository : IEmployeeRepository
{
    private List<Employee> _employees = ...;

    public List<Employee> GetEmployees()
    {
        return _employees;
    }
    // ... other instance methods
}
```


***

### Interview Summary Table: Preparing for Dependency Injection

| Concept | Description | Key Points for Interviews |
| :-- | :-- | :-- |
| **Dependency Injection (DI)** | A design pattern where a class's dependencies are "injected" from an external source (a DI container) rather than being created internally. | "DI is fundamental to modern .NET development. It promotes loose coupling, making code more modular, maintainable, and easier to test by allowing dependencies to be swapped out." |
| **Constructor Injection** | The most common form of DI. Dependencies are provided as parameters to the class's constructor. | "This is the preferred DI pattern for services in MVC controllers and Razor PageModels. The dependencies are injected once when the class is created and are available for its entire lifetime." |
| **Interface (Role in DI)** | A contract that defines a set of methods and properties. Classes depend on interfaces, not on concrete implementations. | "Interfaces are the key to loose coupling in DI. Your controller should depend on `IEmployeeRepository`, not `EmployeeRepository`. This allows you to easily substitute a different implementation, like a `MockEmployeeRepository` for unit testing." |
| **Static Classes \& DI** | Static classes cannot be used with DI because a DI container works by *instantiating* objects, and static classes cannot be instantiated. | "A key prerequisite for making a service injectable is to ensure it's a non-static instance class. You can't inject a static class." |
| **"Extract Interface"** | A common refactoring technique (often built into IDEs like Visual Studio) to automatically create an interface from an existing class's public members. | "This is a practical first step when refactoring a legacy class to use DI. It quickly generates the contract (the interface) that your other components will depend on." |


## Constructor Injection

* **Step 1: Register Services with the DI Container:** Before you can inject a dependency, you must register it with ASP.NET Core's built-in Dependency Injection (DI) container. This is done in `Program.cs` by mapping an interface to its concrete implementation (e.g., `builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>()`).
* **Constructor Injection is the Standard Pattern:** In classes managed by the framework (like MVC Controllers, Razor PageModels, and View Components), constructor injection is the most common and recommended way to receive dependencies.
* **The Injection Process:**

1. Create a constructor for the class that needs the dependency.
2. Add a parameter to the constructor for the dependency's **interface** (e.g., `IEmployeeRepository`).
3. Store the injected object in a private readonly field for use throughout the class.
* **How it Works Automatically:** When a request comes in, the ASP.NET Core framework is responsible for creating an instance of your controller or PageModel. It inspects the constructor, sees that it requires a service like `IEmployeeRepository`, looks in the DI container for the registered implementation (`EmployeeRepository`), creates an instance, and passes it into the constructor.
* **The Pattern is Consistent:** The exact same constructor injection pattern is used for MVC Controllers, Razor PageModels, and View Components, making it a universal skill within the ASP.NET Core ecosystem.


### Illustrations with Code Examples and a Diagram

#### **Diagram: The Constructor Injection Flow**

This diagram illustrates how the ASP.NET Core framework provides a controller with its dependencies.

```
+------------------+     +------------------------+     +---------------------------+
| 1. HTTP Request  | --> | 2. ASP.NET Core        | --> | 3. Needs to create        |
|    For a page    |     |    Routing finds the   |     |    EmployeesController    |
+------------------+     |    correct controller  |     +---------------------------+
                         +------------------------+                 |
                                                                    v
+------------------+     +------------------------+     +---------------------------+
| 6. Controller is | <-- | 5. DI Container finds  | <-- | 4. Framework inspects     |
|    ready to use  |     |    and provides the    |     |    constructor, sees it   |
|    the `_repo`   |     |    `IEmployeeRepository` |     |    needs a dependency     |
+------------------+     +------------------------+     +---------------------------+
```


#### **Code Examples**

**1. Registering the Services (in `Program.cs`)**

This is the essential first step. Here, we're telling the DI container that whenever a class asks for `IEmployeeRepository`, it should provide an instance of `EmployeeRepository`.

```csharp
var builder = WebApplication.CreateBuilder(args);

// Registering services with the DI container
builder.Services.AddSingleton<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();

var app = builder.Build();
//...
```

**2. Injecting into an MVC Controller**

```csharp
public class DepartmentsController : Controller
{
    // 1. Private field to hold the dependency
    private readonly IDepartmentRepository _departmentRepository;

    // 2. The dependency is injected into the constructor
    public DepartmentsController(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public IActionResult Index()
    {
        // 3. Use the injected instance
        var departments = _departmentRepository.GetDepartments();
        return View(departments);
    }
}
```

**3. Injecting Multiple Dependencies into a Razor PageModel**

```csharp
public class EditModel : PageModel
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    // A constructor can accept multiple dependencies
    public EditModel(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
    }

    public void OnGet(int id)
    {
        // Use the injected instances
        Employee = _employeeRepository.GetEmployeeById(id);
        Departments = _departmentRepository.GetDepartments();
    }
}
```


***

### Interview Summary Table: Constructor Injection

| Concept | Description | Key Points for Interviews |
| :-- | :-- | :-- |
| **Service Registration** | The process of telling the DI container which concrete class to provide when an interface is requested. This is done in `Program.cs`. | "Before you can inject a service, you must register it, typically by mapping an interface to a concrete class using methods like `AddSingleton`, `AddScoped`, or `AddTransient` on `builder.Services`." |
| **Constructor Injection** | The pattern of receiving dependencies as parameters in a class's constructor. This is the most common DI pattern in ASP.NET Core. | "Constructor injection is the standard way to provide dependencies to controllers, PageModels, and other services. The dependencies are supplied when the object is created and are available to its entire lifetime." |
| **The DI Container** | The framework component that manages service lifetimes and resolves dependencies. It's responsible for creating and "injecting" the objects. | "The DI container is like a registry of services. When the framework needs to create a controller, it asks the container for the controller's dependencies, and the container provides them based on the initial registration." |
| **Dependency Chains** | When a service that is being injected has its own dependencies. The DI container can resolve these nested dependencies automatically. | "The container can resolve entire dependency graphs. If my `EmployeesController` needs an `IEmployeeRepository`, and `EmployeeRepository` itself needs an `IDepartmentRepository`, the container will correctly instantiate and inject both." |
| **Service Lifetimes** | **Singleton:** One instance for the entire application. **Scoped:** One instance per client request (HTTP request). **Transient:** A new instance is created every time it's requested. | Be ready to explain the difference. **Singleton** is good for a shared in-memory cache or repository. **Scoped** is standard for services that use a database context. **Transient** is for lightweight, stateless services. |
