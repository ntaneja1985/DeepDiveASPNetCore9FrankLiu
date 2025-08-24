# Documenting Web APIs with OpenAPI (MVC Controllers)

## Overview

This video explains how to use OpenAPI to document Web API endpoints built with MVC controllers in ASP.NET Core. The process is very similar to Minimal APIs but applies to controller-based projects.

## Key Points

- Use Visual Studio's ASP.NET Core Web API project template to create a new controller-based API project.
- Install the `Microsoft.AspNetCore.OpenApi` NuGet package to enable OpenAPI generation.
- Configure OpenAPI middleware early in the request pipeline in `Program.cs`.
- Add dependency injection for OpenAPI services with `builder.Services.AddOpenApi()`.
- Access the generated documentation at `/openapi/v1.json`.
- Ensure all controller action methods are properly decorated with HTTP method attributes for discovery.
- Enabling the launch browser setting makes it easier to test and view documentation directly.
- OpenAPI document shows all endpoints, their HTTP methods, response types, and possible status codes.


## Implementation Steps

1. Create a new ASP.NET Core Web API project using Visual Studio's template.
2. Install the OpenAPI NuGet package:

```bash
Install-Package Microsoft.AspNetCore.OpenApi
```

3. In `Program.cs`, register the OpenAPI services and middleware:

```csharp
builder.Services.AddOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
```

4. Decorate controller action methods with `[HttpGet]`, `[HttpPost]`, etc., to enable OpenAPI discovery.
5. Run the project and navigate to `/openapi/v1.json` to see raw JSON documentation.

## Interview Quick Reference

| Concept | Importance | Key Takeaway |
| :-- | :-- | :-- |
| **OpenAPI for Controllers** | Provides a standardized, auto-generated API documentation for controllers just as for Minimal APIs. | "OpenAPI is framework-agnostic within ASP.NET Core and works seamlessly with both Minimal APIs and MVC controllers, making API docs consistent across styles." |
| **NuGet Package Dependency** | Installation of OpenAPI package is required in each project hosting the API endpoints for documentation to work. | "I ensure that every API project has the appropriate OpenAPI NuGet package installed to enable automatic documentation generation." |
| **Middleware Configuration** | Adding OpenAPI middleware early ensures documentation requests are handled promptly with minimal overhead. | "I configure OpenAPI middleware early in the pipeline to efficiently serve the spec file without unnecessary interception by other middleware." |
| **HTTP Method Attributes** | Proper use of HTTP method attributes on controllers enables accurate endpoint discovery and documentation. | "I always decorate controller actions with HTTP method attributes to ensure they are correctly included in OpenAPI documentation." |
| **Developer Experience** | Enabling browser launch and easy endpoint testing improves productivity and eases API maintenance. | "I configure API projects to launch a browser by default in development, so I can quickly view and verify OpenAPI docs during development." |


# Visualizing OpenAPI Documentation with Swagger UI

## Overview

This video shows how to use the Swashbuckle.AspNetCore NuGet package to generate an interactive Swagger UI for viewing and testing your OpenAPI documentation in a web browser. This enhances the developer experience by providing a user-friendly interface on top of the raw JSON spec.

## Key Points

- Install the `Swashbuckle.AspNetCore` NuGet package.
- Configure Swagger UI middleware in `Program.cs`.
- Point the Swagger UI middleware to the OpenAPI JSON endpoint (default: `/openapi/v1.json`).
- Launch the application and navigate to `/swagger` to access the UI.
- The UI lists all endpoints, grouped by controller or tags, with HTTP methods and response types.
- Allows executing API endpoints directly with an easy "Try it out" button.


## Code Implementation

```csharp
// Program.cs
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "API V1");
});
```


## Interview Quick Reference

| Concept | Why It's Important | Takeaway |
| :-- | :-- | :-- |
| **Swagger UI Integration** | Provides interactive, auto-generated API documentation enabling testing and exploration directly in the browser. | "I use the Swashbuckle package to quickly add a developer-friendly UI to my APIs, improving testing and exploration capabilities." |
| **Middleware Configuration** | Placing Swagger UI middleware after OpenAPI middleware ensures the UI fetches the up-to-date spec dynamically. | "Middleware order matters: Swagger UI depends on the OpenAPI spec being served, so I configure `UseSwaggerUI` after `MapOpenApi`." |
| **Default Endpoint** | The Swagger UI endpoint is typically `/swagger`, a standard convention familiar to many developers. | "I configure my APIs so developers can easily find and use Swagger UI by navigating to `/swagger`." |
| **'Try It Out' Feature** | Allows developers to execute API calls directly from the UI, receiving live responses without external tools. | "The 'Try it out' feature enables real-time testing within the UI, accelerating development and debugging." |


# Providing Details for OpenAPI in Controller-Based APIs

## Overview

This video explains how to enrich the OpenAPI documentation for Web APIs built with MVC controllers. Similar to the process for Minimal APIs, developers can use a variety of attributes on controller action methods to add descriptive metadata, group endpoints, and explicitly declare response types, leading to more comprehensive and user-friendly documentation.

## Key Points

- **Methodology**: Descriptive metadata is added using attributes directly on the action methods within a controller.
- **Consistency with Minimal APIs**: The attributes used are largely the same as those for Minimal APIs, providing a consistent way to document APIs regardless of how they are built.
- **Common Attributes for Documentation**:
    - `[Tags("...")]`: Groups related action methods under a specific category in the Swagger UI, improving organization and discoverability.
    - `[EndpointSummary("...")]`: Provides a short, human-readable summary that appears next to the endpoint in the UI.
    - `[EndpointDescription("...")]`: Offers a more detailed explanation of the endpoint's purpose and behavior.
    - `[EndpointName("...")]`: Sets the `operationId` in the OpenAPI specification, which is particularly useful for client code generation tools.
    - `[ProducesResponseType(...)]`: Explicitly documents all possible responses an action can return, including both success statuses (like `204 No Content`) and error statuses (like `404 Not Found`) that the framework might not automatically discover.


## Code Implementation

This example demonstrates how to apply various descriptive attributes to an action method in a controller.

```csharp
[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpPut("{id}")]
    [Tags("Changing")]
    [EndpointName("UpdateForecast")]
    [EndpointSummary("Updates an existing weather forecast.")]
    [EndpointDescription("Updates a specific weather forecast by its ID. Returns No Content on success.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, WeatherForecast forecast)
    {
        if (id != forecast.Id)
        {
            // This is just an example; real logic would check a database.
            return NotFound(); 
        }

        // ... logic to update the forecast
        
        return NoContent();
    }
}
```


## Interview Quick Reference

| Concept | Why It's Important | Key Takeaway for Interviews |
| :-- | :-- | :-- |
| **Attributes for API Documentation** | These attributes allow developers to embed rich metadata directly into the code, ensuring that the documentation is always synchronized with the implementation. | "I use attributes like `[Tags]`, `[EndpointSummary]`, and `[ProducesResponseType]` directly on my controller actions to create comprehensive, self-documenting APIs. This keeps the documentation and code in one place." |
| **`[Tags]`** | Organizes endpoints into logical groups within documentation UIs like Swagger. This is crucial for making large APIs easy to navigate and understand. | "The `[Tags]` attribute is essential for organizing my API documentation. I group endpoints by resource or functionality, which makes it much easier for consumers to find the endpoints they need." |
| **`[ProducesResponseType]`** | It explicitly declares all possible HTTP responses, which is critical for a complete API contract. It helps clients understand how to handle both success and error cases. | "To provide a complete and reliable API contract, I use `[ProducesResponseType]` to document every possible status code an action can return, especially error codes like 404 Not Found or success codes like 204 No Content that can't be inferred from the return type alone." |
| **Discoverability vs. Explicitness** | While ASP.NET Core can discover much about an API automatically, relying on explicit attributes for documentation creates a more robust and unambiguous specification. | "While automatic discovery is a great starting point, I believe in being explicit. Using attributes to define summaries and all possible responses removes ambiguity and results in higher-quality, more reliable documentation." |


# Web API Versioning with Controllers in ASP.NET Core

## Overview

This video explains how to implement API versioning for ASP.NET Core Web API endpoints created using MVC controllers. Versioning enables the coexistence of multiple API versions to avoid breaking existing clients when introducing changes.

## Key Points

- Unlike Minimal APIs, controller-based versioning requires an additional MVC support package.
- Separate versions of controllers can be organized into different folders or namespaces (e.g., "v1", "v2").
- Use the `[ApiVersion]` attribute on controllers to declare their version.
- Configure versioning services in `Program.cs` using `AddApiVersioning()`, setting default versions, reporting, and version reading options.
- Use query string, headers, or media type to let clients specify the desired API version.
- Handle duplicate endpoint names and operation IDs carefully to avoid conflicts.
- The versioned endpoints use routes like `/endpoint?api-version=1.0` to route requests appropriately.
- MVC API versioning requires `AddMvc().AddApiVersioning()` registration.


## Implementation Steps

1. Install NuGet Packages:

```bash
Install-Package Asp.Versioning.Http
Install-Package Asp.Versioning.Mvc.ApiExplorer
```

2. Structure your controllers in folders/namespaces per version (e.g., `Controllers.V1`, `Controllers.V2`).
3. Decorate controllers with versions:

```csharp
[ApiVersion("1.0")]
public class WeatherForecastV1Controller : ControllerBase
{
    // ...
}

[ApiVersion("2.0")]
public class WeatherForecastV2Controller : ControllerBase
{
    // ...
}
```

4. Configure versioning in `Program.cs`:

```csharp
builder.Services.AddMvc().AddApiVersioning(options => {
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"));
});
```

5. Run and test:
    * Access routes with query string parameter, e.g., `/weatherforecast?api-version=1.0` or `?api-version=2.0`.
    * The versioned controllers handle requests accordingly.

## Interview Quick Reference

| Concept | Importance | Takeaway |
| :-- | :-- | :-- |
| **API Versioning for Controllers** | Enables backward compatibility and safe API evolution for controller-based APIs. | "I use versioned controllers with appropriate attributes and configure MVC versioning support to manage different API versions." |
| **Separate Controller Versions** | Organizing code in version-specific namespaces/folders improves maintainability. | "I isolate version implementations in separate folders or namespaces, keeping code clean and easier to evolve." |
| **Version Reading Methods** | Specifying how the API reads the version from query strings or headers enhances flexibility for clients. | "I configure multiple version readers like query string and headers to maximize client compatibility." |
| **Register MVC with Versioning** | MVC API versioning requires explicitly adding versioning support via MVC services. | "Unlike minimal APIs, controller-based API versioning requires calling `AddMvc().AddApiVersioning()` to enable full support." |


# Documenting Multiple Versions of Controller-Based APIs

## Overview

This video explains how to set up OpenAPI and Swagger UI to correctly document a controller-based Web API that has multiple versions. The process involves configuring separate OpenAPI documents for each version, grouping controllers by version, and integrating the API versioning information with the API explorer so that Swagger UI becomes version-aware.

## Key Points

- **Multiple OpenAPI Documents**: In `Program.cs`, you must register a separate OpenAPI document for each API version (e.g., "v1", "v2") using `builder.Services.AddOpenApi()`. Each registration must include a filter (`ShouldInclude`) to only include endpoints belonging to that version's group.
- **Grouping Controllers**: You must assign each versioned controller to its corresponding group using the `[ApiExplorerSettings(GroupName = "...")]` attribute. The `GroupName` string must match the one used in the OpenAPI document filter.
- **API Explorer Integration**: It is crucial to add `.AddApiExplorer()` to the `AddApiVersioning()` service registration chain. This component bridges the gap between the API versioning system and the OpenAPI generator, allowing Swagger to understand the versions and add the necessary version parameters to its UI.
- **Swagger UI Configuration**: The `UseSwaggerUI` middleware must be configured with an endpoint for each version's OpenAPI JSON file. This populates the version dropdown menu in the Swagger UI.


## Code Implementation

### 1) Configure Services in `Program.cs`

Set up the versioning services, API Explorer, and the separate OpenAPI documents.

```csharp
// Program.cs

// 1. Configure API versioning and connect it to the API Explorer
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    // ... other versioning options
}).AddApiExplorer(options =>
{
    // Optional: Configure API Explorer if needed, but defaults are often sufficient.
});

// 2. Define a separate OpenAPI document for each version
builder.Services.AddOpenApi("v1", options =>
{
    options.ShouldInclude = apiDesc => apiDesc.GroupName == "v1";
});

builder.Services.AddOpenApi("v2", options =>
{
    options.ShouldInclude = apiDesc => apiDesc.GroupName == "v2";
});
```


### 2) Configure Middleware in `Program.cs`

Configure Swagger UI to display a dropdown for switching between the v1 and v2 documents.

```csharp
// Program.cs

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        // Add an endpoint for each version to the Swagger UI dropdown
        c.SwaggerEndpoint("/openapi/v1.json", "API v1");
        c.SwaggerEndpoint("/openapi/v2.json", "API v2");
    });
}
```


### 3) Group Controllers by Version

Apply the `ApiExplorerSettings` attribute to each controller class.

```csharp
// In Controllers/v1/WeatherForecastV1Controller.cs
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
public class WeatherForecastV1Controller : ControllerBase
{
    // ... actions
}

// In Controllers/v2/WeatherForecastV2Controller.cs
[ApiVersion("2.0")]
[ApiExplorerSettings(GroupName = "v2")]
public class WeatherForecastV2Controller : ControllerBase
{
    // ... actions
}
```


## Interview Quick Reference

| Concept | Why It's Important | Key Takeaway for Interviews |
| :-- | :-- | :-- |
| **`AddApiExplorer()`** | This service is the critical link between the API versioning system and the OpenAPI/Swagger generation tools. Without it, Swagger UI will not be aware of the versioning parameters. | "To make OpenAPI and Swagger fully aware of my API versions, I always register the API Explorer by chaining `.AddApiExplorer()` to my `AddApiVersioning()` call. This ensures the version parameter appears in the UI." |
| **`[ApiExplorerSettings(GroupName = "...")]`** | This attribute is how you assign a controller and all of its actions to a specific documentation group. This is the mechanism used to filter endpoints into the correct versioned OpenAPI document. | "I use the `[ApiExplorerSettings]` attribute on my controllers to assign them to a `GroupName` like 'v1' or 'v2'. This allows the OpenAPI generator to correctly partition the endpoints into separate, version-specific documents." |
| **Separate OpenAPI Documents** | Generating a distinct OpenAPI specification file for each version of the API is a best practice. It provides a clean and unambiguous contract for each version. | "I configure my application to generate a separate OpenAPI document for each supported API version. This is achieved by calling `AddOpenApi` multiple times and using the `ShouldInclude` filter to select endpoints by their group name." |
| **Swagger UI Version Dropdown** | Configuring the Swagger UI to display a dropdown of available API versions provides a seamless and intuitive experience for developers who need to explore or test different versions of the API. | "In `UseSwaggerUI`, I configure a `SwaggerEndpoint` for each versioned document. This creates the version selector dropdown in the UI, making it easy for consumers to switch between and test different API versions." |

