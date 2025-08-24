# Documenting Minimal APIs with OpenAPI

## Overview

This video introduces the concept of documenting Web APIs using the **OpenAPI Specification** (formerly known as Swagger). It explains why API documentation is crucial and demonstrates the basic steps to generate an OpenAPI document for a Minimal API project in ASP.NET Core.

## Why Document APIs?

- **Clear Specification**: Provides a contract for consumers (other developers, teams, or third parties) detailing how to use the API, including endpoint URLs, required parameters, data schemas, and possible responses.
- **Enables Testing**: The OpenAPI document is machine-readable and can be used by tools like Swagger UI or Postman to automatically generate an interactive UI for testing the API endpoints without writing any client code.


## Key Points

- **Standardization**: OpenAPI is a widely adopted standard for describing RESTful APIs.
- **NuGet Package**: The core functionality is provided by the `Microsoft.AspNetCore.OpenApi` NuGet package.
- **Simple Configuration**: Basic documentation can be enabled with just two lines of code in `Program.cs`.
- **Automatic Discovery**: The framework inspects your code—including endpoints, filters, and return types—to automatically generate the specification.
- **Limitations**: The auto-discovery process is not perfect. It may not be able to infer all possible responses, especially those determined by complex business logic within filters (e.g., a 404 Not Found response). Developers may need to manually add descriptions for these cases.


## Code Implementation

### 1) Install the OpenAPI NuGet Package

In your Web API project, install the necessary package from NuGet.

```bash
# Package Manager Console
Install-Package Microsoft.AspNetCore.OpenApi

# or .NET CLI
dotnet add package Microsoft.AspNetCore.OpenApi
```


### 2) Configure OpenAPI in `Program.cs`

Add the OpenAPI services and map the endpoint that serves the documentation file. This is typically done only in the development environment.

```csharp
// Program.cs

var builder = WebApplication.CreateBuilder(args);

// 1. Add the OpenAPI services to the DI container
builder.Services.AddOpenApi();

var app = builder.Build();

// 2. Map the endpoint to serve the openapi.json file
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ... your other app configurations and endpoint mappings

app.Run();
```


### Accessing the Generated Document

Once configured and running, the OpenAPI specification file can be accessed at the URL: `/openapi/v1.json`

## Interview Quick Reference

| Concept | Why It's Important | Key Takeaway for Interviews |
| :-- | :-- | :-- |
| **OpenAPI Specification** | It provides a language-agnostic, standard interface for describing RESTful APIs, which enables both humans and computers to discover and understand the capabilities of a service without access to source code. | "I use OpenAPI to create a standardized, machine-readable contract for my APIs. This documentation is crucial for client developers to understand how to interact with the endpoints and it enables automated tooling." |
| **API Documentation** | Good documentation is essential for API adoption and usability. It reduces the friction for developers trying to integrate with your service and serves as a single source of truth for the API's behavior. | "Clear API documentation is a priority in my projects. By auto-generating an OpenAPI spec from my code, I ensure the documentation is always in sync with the implementation, which is a major advantage over manual documentation." |
| **`Microsoft.AspNetCore.OpenApi`** | This is the official Microsoft package for integrating OpenAPI generation into ASP.NET Core applications. It provides the necessary services and middleware. | "For documenting my ASP.NET Core APIs, I use the `Microsoft.AspNetCore.OpenApi` package. It's lightweight and integrates seamlessly by providing the `AddOpenApi()` and `MapOpenApi()` extension methods." |
| **Automatic Discovery and Its Limits** | The framework can infer a lot about your API from the code, but it's not foolproof. Complex logic can hide certain response types from the generator. | "While the automatic discovery of endpoints and schemas is very powerful, I'm aware of its limitations. For complex scenarios, like returning a 404 from a custom filter, I know I may need to provide explicit metadata to ensure the documentation is 100% accurate." |


# How OpenAPI Middleware Works

## Overview

This video provides a high-level explanation of the mechanics behind OpenAPI in ASP.NET Core. It clarifies that OpenAPI documentation is generated and served by a dedicated middleware component that sits early in the request pipeline, intercepting and handling requests for the API specification without involving the rest of the application's endpoint logic.

## Key Points

- **Middleware Implementation**: The core of OpenAPI integration (`app.MapOpenApi()`) is a middleware component.
- **Early Pipeline Position**: This middleware is placed at the beginning of the request pipeline, before the routing and endpoint execution middleware.
- **Request Interception**: It inspects the URL of incoming requests. If a request targets the documentation endpoint (e.g., `/openapi/v1.json`), the middleware takes control.
- **Dynamic Endpoint Discovery**: Upon intercepting a documentation request, the middleware reflects on the application to discover all registered API endpoints, their routes, parameters, and expected responses.
- **On-the-Fly Generation**: It uses the discovered information to generate a JSON document that conforms to the OpenAPI Specification.
- **Short-Circuiting the Pipeline**: Once the document is generated, the middleware writes it to the response and stops further processing. The request does not proceed to the routing or endpoint middleware, making the process efficient.


## Conceptual Flow of a Documentation Request

1. A client sends a request to `GET /openapi/v1.json`.
2. The request enters the ASP.NET Core middleware pipeline.
3. The OpenAPI middleware, being one of the first in the chain, identifies the request URL.
4. It intercepts the request, preventing it from going further.
5. It dynamically scans the application to find all API endpoints and their metadata.
6. It builds a valid OpenAPI JSON document from this metadata.
7. It writes the JSON document to the HTTP response and completes the request.

## Interview Quick Reference

| Concept | Why It's Important | Key Takeaway for Interviews |
| :-- | :-- | :-- |
| **OpenAPI Middleware** | It provides a non-invasive and efficient way to add on-the-fly documentation generation to an API without interfering with the application's core logic. | "The OpenAPI functionality in ASP.NET Core is implemented as middleware that sits early in the pipeline. It intercepts requests for the spec file, generates it dynamically, and returns it, short-circuiting the request." |
| **Endpoint Discovery (Reflection)** | This is the mechanism that allows the documentation to be automatically generated and kept in sync with the code. It eliminates the need for manual documentation, which can easily become outdated. | "The middleware uses reflection at runtime to discover all the registered endpoints, their routes, HTTP methods, parameters, and return types. This is how the generated documentation always reflects the actual state of the API." |
| **Short-Circuiting the Pipeline** | This is a key performance optimization. Requests for documentation are handled and completed early, avoiding the unnecessary overhead of passing through the full routing and endpoint execution pipeline. | "A key feature of the OpenAPI middleware is that it short-circuits the pipeline. Once it generates the documentation, it sends the response directly. This is highly efficient as the request doesn't need to proceed to the routing and endpoint execution stages." |
| **Adherence to Specification** | The primary job of the middleware is to translate the application's endpoint metadata into a document that strictly conforms to the OpenAPI standard. | "The real complexity within the middleware is its responsibility to build a document that strictly adheres to the OpenAPI Specification. This standardization is what allows a wide ecosystem of tools, like Swagger UI and Postman, to understand and work with the API." |


# Using OpenAPI with Postman for API Testing

## Overview

This video demonstrates a practical use case for the generated OpenAPI specification file: importing it into Postman to automatically create a collection of testable API requests. This process bridges the gap between API documentation and interactive testing, allowing developers to quickly validate their endpoints without manually creating each request.

## Key Points

- **Consuming the Spec**: The machine-readable OpenAPI JSON document, while not very human-friendly, is designed to be consumed by tools like Postman.
- **Automated Collection Generation**: Postman can parse an OpenAPI document and generate a complete collection, including folders for different endpoint groups, pre-configured HTTP verbs, URLs, and placeholders for parameters.
- **Environment Variables**: The generated collection often uses variables (e.g., `{{baseUrl}}`) for the server's host address. This is a best practice that makes it easy to switch between different environments (like local, staging, and production) by simply changing the variable's value.
- **Static Snapshot**: The imported collection is a static snapshot of the API at the time of import. If the API is updated, the Postman collection must be refreshed by deleting the old one and re-importing the new OpenAPI specification.


## How to Import OpenAPI into Postman

1. Ensure your Web API project is running and the `/openapi/v1.json` endpoint is accessible.
2. Copy the entire raw JSON content from the browser.
3. In Postman, navigate to **File > Import**.
4. In the Import dialog, select the **Raw text** tab.
5. Paste the copied JSON into the text area.
6. Postman will recognize it as an OpenAPI specification. Click **Import**.
7. A new collection will be created, organized by the API's endpoints.
8. Check the collection's **Variables** tab to see or modify the `baseUrl`.

## Interview Quick Reference

| Concept | Why It's Important | Key Takeaway for Interviews |
| :-- | :-- | :-- |
| **Tooling for OpenAPI** | The real power of the OpenAPI standard lies in its vast ecosystem of tools. Tools like Postman, Swagger UI, and code generators use the spec to automate tasks, accelerating the development and testing lifecycle. | "I leverage the OpenAPI ecosystem by importing the spec into tools like Postman. This automatically generates a comprehensive test collection, saving significant time and ensuring my tests are perfectly aligned with the API's contract." |
| **"Documentation as Code" Workflow** | Treating the auto-generated OpenAPI spec as the source of truth for both documentation and testing ensures consistency. When the code changes, the documentation and tests can be updated in a single, streamlined process. | "My workflow treats API documentation as code. After any change to an endpoint, I regenerate the OpenAPI spec and re-import it into Postman. This keeps my code, documentation, and test suites in perfect sync." |
| **API Test Collections** | A Postman collection is a structured way to group, organize, and share API requests. Generating it from a spec provides a solid foundation for building out a full regression test suite. | "Generating a Postman collection from the OpenAPI spec gives me an excellent starting point for API testing. I can then build upon this foundation by adding assertions and scripting more complex test scenarios." |
| **Environment Variables in Testing** | Using variables like `baseUrl` decouples the test suite from a specific environment. This makes the tests portable and reusable across different stages of the development lifecycle (local, dev, staging, prod). | "The generated Postman collection correctly uses a `baseUrl` variable for the host. This is a critical best practice that allows me to run the same test suite against any environment just by switching the variable's value, which is essential for CI/CD pipelines." |


# Visualizing OpenAPI with Swagger UI

## Overview

This video introduces Swagger UI, a popular tool that generates an interactive, browser-based user interface from an OpenAPI specification file. It serves as a powerful alternative to tools like Postman, allowing developers and API consumers to visualize, explore, and test API endpoints directly within a web browser.

## Key Points

- **Interactive UI**: Swagger UI renders the raw OpenAPI JSON document as a user-friendly HTML page, listing all available API endpoints.
- **Installation**: The functionality is added by installing the `Swashbuckle.AspNetCore` NuGet package, a widely-used, third-party library.
- **Middleware Configuration**:
    - Swagger UI is enabled by adding the `app.UseSwaggerUI()` middleware in `Program.cs`.
    - This middleware must be placed *after* the OpenAPI generation middleware (`app.MapOpenApi()`) because it consumes the generated JSON file.
    - It needs to be configured to point to the correct OpenAPI JSON endpoint (e.g., `/openapi/v1.json`).
- **"Try it out" Functionality**: The UI includes a feature that allows users to fill in parameters, craft request bodies, and execute API calls directly from the browser, showing the response code, headers, and body.


## Code Implementation

### 1) Install the Swashbuckle NuGet Package

In your Web API project, install the `Swashbuckle.AspNetCore` package.

```bash
# Package Manager Console
Install-Package Swashbuckle.AspNetCore

# or .NET CLI
dotnet add package Swashbuckle.AspNetCore
```


### 2) Configure Swagger UI in `Program.cs`

Add and configure the Swagger UI middleware. Note that `UseSwaggerUI` is separate from `AddSwaggerGen` (which is another part of Swashbuckle for generating the spec itself, often used as an alternative to `AddOpenApi`). The key is that the UI consumes a spec endpoint.

```csharp
// Program.cs

var builder = WebApplication.CreateBuilder(args);

// Add services for OpenAPI/Swagger
builder.Services.AddOpenApi(); // Or builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Middleware to generate the openapi.json file
    app.MapOpenApi();


    // Middleware to serve the Swagger UI, pointing to the JSON file
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "My API V1");
    });
}

// ... your other app configurations and endpoint mappings

app.Run();
```


### Accessing the UI

Once configured and running, the interactive Swagger UI page is typically available at the `/swagger` URL.

## Interview Quick Reference

| Concept | Why It's Important | Key Takeaway for Interviews |
| :-- | :-- | :-- |
| **Swagger UI** | It provides an interactive, self-documenting UI for APIs, dramatically improving the developer experience for API consumers and accelerating the testing and integration process. | "I integrate Swagger UI into my APIs to provide an interactive documentation portal. It allows consumers to explore and test endpoints directly in the browser, which is excellent for discovery and initial integration." |
| **`Swashbuckle.AspNetCore`** | This is the de facto standard library for integrating Swagger and OpenAPI into ASP.NET Core projects. It's robust and provides tools for both spec generation and UI rendering. | "Swashbuckle is my go-to library for adding Swagger support. It's a comprehensive package that handles both the OpenAPI spec generation (`SwaggerGen`) and the UI rendering (`SwaggerUI`)." |
| **Middleware Order** | The `UseSwaggerUI` middleware is a consumer of the OpenAPI specification. Therefore, it must be placed after the middleware that generates and serves that specification file. | "The order of middleware is critical. `UseSwaggerUI` must come after the middleware that serves the OpenAPI JSON file, because the UI dynamically fetches that file to render the documentation." |
| **"Try it out" Feature** | This turns static documentation into a live testing tool, allowing for immediate validation of API endpoints without needing an external client like Postman. It's a powerful feature for both developers and consumers of the API. | "The 'Try it out' feature in Swagger UI is invaluable for quick, ad-hoc testing. It lets me execute requests and see live responses directly from the documentation, which significantly speeds up my development and debugging workflow." |


# Providing Details for OpenAPI in Minimal APIs

## Overview

This video demonstrates how to add more descriptive details to the OpenAPI documentation for Minimal APIs. While the framework can automatically discover a lot of information, developers can use specific attributes and methods to provide richer metadata, such as tags for grouping, summaries, and explicit response types that the generator might miss.

## Key Points

- **Need for Customization**: Automatic discovery is powerful but has limitations. Custom metadata is often needed to describe business logic, group related endpoints, or document all possible API responses (especially error codes).
- **Using Attributes**: Metadata can be applied directly to the route handler (the lambda expression or method). This requires separating the handler from the mapping call (e.g., `app.MapGet(...)`).
- **Common Attributes for Documentation**:
    - `[Tags("...")]`: Groups related endpoints under a specific heading in tools like Swagger UI.
    - `[EndpointName("...")]`: Assigns a unique ID (`operationId`) to the endpoint. Useful for code generation clients.
    - `[EndpointSummary("...")]`: Provides a short, human-readable summary of what the endpoint does. This is often displayed prominently in documentation UIs.
    - `[EndpointDescription("...")]`: Offers a more detailed description of the endpoint's functionality.
    - `[ProducesResponseType(...)]`: Explicitly declares a possible HTTP status code and response type that the endpoint can return, which is crucial for documenting error conditions like 404 Not Found.
- **Fluent API Alternative**: Minimal APIs also support a fluent API syntax (e.g., `.WithTags("...")`, `.WithSummary("...")`) which can be chained to the endpoint mapping call as an alternative to attributes.


## Code Implementation

This example shows how to apply various descriptive attributes to a Minimal API endpoint handler.

```csharp
// The endpoint handler is defined as a local function or lambda
// to allow attributes to be applied.
var getDepartmentsHandler =
    [Tags("Web API - Departments")]
    [EndpointName("GetDepartments")]
    [EndpointSummary("Get all departments")]
    [EndpointDescription("Gets a list of all departments, optionally filtered by name.")]
    async (IDepartmentsRepository repository, string? filter) =>
    {
        return await repository.GetDepartmentsAsync(filter);
    };

// The handler is then passed to the mapping method.
app.MapGet("/departments", getDepartmentsHandler);


// Example for documenting a 404 response
var updateDepartmentHandler =
    [Tags("Web API - Departments")]
    [EndpointSummary("Update a department")]
    // Explicitly document the 404 Not Found response
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    async (int id, Department department, IDepartmentsRepository repository) =>
    {
        // ... implementation
        await repository.UpdateDepartmentAsync(department);
        return Results.NoContent();
    };

app.MapPut("/departments/{id}", updateDepartmentHandler)
   .AddEndpointFilter<EnsureDepartmentExistsFilter>(); // This filter can return 404
```


## Interview Quick Reference

| Concept | Why It's Important | Key Takeaway for Interviews |
| :-- | :-- | :-- |
| **Enriching API Documentation** | Auto-generated documentation is a starting point. Manually adding descriptive metadata like summaries, descriptions, and explicit response types makes the API significantly easier to understand and consume. | "I go beyond basic auto-generation by enriching my OpenAPI documentation with attributes like `EndpointSummary` and `ProducesResponseType`. This creates a comprehensive, self-contained guide for any developer consuming my API." |
| **`[Tags]` Attribute** | It organizes a large number of endpoints into logical groups in the documentation UI, which is essential for discoverability and usability in complex APIs. | "I use the `[Tags]` attribute to group my endpoints by resource, like 'Products' or 'Orders'. This makes the Swagger UI much more organized and easier for developers to navigate." |
| **`[ProducesResponseType]`** | This attribute is critical for explicitly documenting all possible success and error responses an endpoint can return, especially those that can't be inferred from the action's return signature (e.g., a 404 from a filter). | "To ensure my API contract is complete, I use `[ProducesResponseType]` to document all possible outcomes, including error codes like 400, 404, or 500. This is crucial for clients to build robust error-handling logic." |
| **Attributes vs. Fluent API** | ASP.NET Core provides two ways to add metadata (attributes or chained methods). Knowing both shows a deeper understanding of the framework's flexibility. | "For Minimal APIs, I can add metadata using either attributes on the handler or fluent methods like `.WithTags()` chained to the endpoint mapping. I often prefer attributes because the syntax is consistent with how I'd decorate a controller-based action method." |
| **Documentation-Driven Development** | The act of carefully documenting endpoints often leads to better API design. Thinking about how an endpoint will be used and what it should return helps clarify its purpose and contract. | "I find that taking the time to thoroughly document my API with these attributes often helps me refine the API design itself. Clearly defining the summaries, parameters, and responses forces me to think from the consumer's perspective." |

# API Versioning in Minimal APIs

## Overview

This video explains how to implement API versioning in an ASP.NET Core Minimal API project. Versioning is crucial for evolving an API over time by allowing you to introduce breaking changes in new versions while maintaining backward compatibility for existing clients. The process involves installing specific NuGet packages, configuring versioning services, and associating endpoints with specific versions.

## Why is API Versioning Necessary?

- **Managing Breaking Changes**: When you need to make a change that would break existing clients (e.g., altering a response model), you can release it as a new version.
- **Supporting Multiple Client Versions**: It allows older clients (like a v1 mobile app) to continue using the old version of the API, while new clients can take advantage of the new version.
- **Clear API Evolution**: It provides a clear, structured path for the API's lifecycle, including introducing new features and deprecating old ones.


## Key Steps for Implementation

1. **Install NuGet Packages**:
    - `Asp.Versioning.Http`: Provides the core versioning functionality for HTTP APIs.
    - `Asp.Versioning.Mvc.ApiExplorer`: Needed for integrating versioning with API exploration tools like OpenAPI and Swagger.
2. **Configure API Versioning Services**:
In `Program.cs`, use `builder.Services.AddApiVersioning()` to configure how versions are read and what the default behavior should be.
3. **Create an `ApiVersionSet`**:
An `ApiVersionSet` defines the collection of API versions that a group of endpoints supports. This is typically created once in `Program.cs` and shared among related endpoints.
4. **Associate Endpoints with Versions**:
Use the `.WithApiVersionSet()` and `.MapToApiVersion()` extension methods on your endpoint mappings to link a specific endpoint implementation to a specific version.

## Code Implementation

### 1) Configure Versioning in `Program.cs`

```csharp
// Program.cs

// 1. Add and configure the versioning services
builder.Services.AddApiVersioning(options =>
{
    // Assume a default version when one is not specified
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    // Report the supported versions in the 'api-supported-versions' header
    options.ReportApiVersions = true;
    // Configure how the version is read from the request (e.g., query string, header)
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Api-Version")
    );
});

// ... Add API Explorer for Swagger/OpenAPI if needed

var app = builder.Build();

// 2. Create a version set for a group of endpoints
var departmentVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .HasApiVersion(new ApiVersion(2, 0))
    .ReportApiVersions()
    .Build();

// ... Map your endpoints using the version set (done in your endpoint definition file)

app.Run();
```


### 2) Map Versioned Endpoints

In your endpoint definition file (e.g., `DepartmentEndpoints.cs`), pass the `ApiVersionSet` and map each handler to its specific version.

```csharp
// In DepartmentEndpoints.cs

// Version 1 of the endpoint
app.MapGet("/departments", V1_GetDepartmentsHandler)
   .WithApiVersionSet(departmentVersionSet)
   .MapToApiVersion(1);

// Version 2 of the endpoint (with a different handler)
app.MapGet("/departments", V2_GetDepartmentsHandler)
   .WithApiVersionSet(departmentVersionSet)
   .MapToApiVersion(2);
```


## Interview Quick Reference

| Concept | Why It's Important | Key Takeaway for Interviews |
| :-- | :-- | :-- |
| **API Versioning** | It's a fundamental strategy for managing the lifecycle of an API, allowing for non-disruptive evolution and ensuring backward compatibility for existing clients. | "I use API versioning to safely introduce breaking changes. By creating a new version, I can evolve the API for new clients while allowing existing clients to continue functioning on the older, stable version." |
| **`Asp.Versioning` Libraries** | This is the official and standard set of libraries for implementing versioning in ASP.NET Core. It's robust and supports multiple versioning schemes. | "For API versioning in .NET, I use the `Asp.Versioning.*` packages. They provide a comprehensive framework for defining, reading, and routing requests based on version." |
| **API Version Reader (`ApiVersionReader`)** | This component determines how the API version is extracted from an incoming request. Common methods include reading from the query string, a URL segment, or a custom header. | "I configure an `ApiVersionReader` to define how clients should specify the version. I often combine a query string reader (`?api-version=2.0`) and a header reader (`X-Api-Version: 2.0`) to offer flexibility to consumers." |
| **`ApiVersionSet`** | In Minimal APIs, this object groups related endpoints and declares which versions they collectively support. It's a key piece of the configuration puzzle. | "An `ApiVersionSet` is used to define and manage a group of supported versions for a set of related endpoints. I then associate my endpoint mappings with this set to enable version-aware routing." |
| **`MapToApiVersion()`** | This method is the final step that links a specific route handler implementation to a specific API version, allowing multiple handlers to coexist on the same route template. | "The `MapToApiVersion()` method is what resolves routing ambiguity. It allows me to have, for example, two `GET /departments` endpoints, and it directs the request to the correct handler based on the version provided by the client." |


# Documenting Multiple Versions in Minimal APIs with OpenAPI

## Overview

This video explains how to configure OpenAPI documentation to support multiple API versions in a Minimal API project. By default, OpenAPI generates documentation for only one version, so extra configuration is needed to generate separate specs for each version.

## Key Points

- You need to define multiple OpenAPI document names matching your API versions (e.g., "v1", "v2").
- Use the `AddOpenApi` method multiple times in `Program.cs` to create different docs for each version.
- Filter endpoints into each version's document by matching their group names.
- Assign endpoints to specific version groups via `.WithGroupName("v1")` or similar.
- You can customize endpoint summaries and descriptions to reflect their version.
- Update Swagger UI to include and correctly display multiple OpenAPI versions.
- Ensure the API Explorer package is installed and configured to support versioning metadata.
- Swagger UI allows selecting different API versions and testing endpoints interactively.
- Query strings or headers can specify API version at runtime, controlling which version handles the request.


## Implementation Steps

1. Define versioned OpenAPI docs:
```csharp
builder.Services.AddOpenApi("v1",o => {
    o.ShouldInclude(new Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription { GroupName = "v1" });
});

builder.Services.AddOpenApi("v2", o => {
    o.ShouldInclude(new Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription { GroupName = "v2" });
});
```

2. Assign group names to endpoints:
```csharp
app.MapGet("/departments", handlerV1)
    .WithGroupName("v1");

app.MapGet("/departments", handlerV2)
    .WithGroupName("v2");
```

3. Configure Swagger UI to list both versions:
```csharp
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/openapi/v1.json", "API Version 1");
    c.SwaggerEndpoint("/openapi/v2.json", "API Version 2");
});
```

4. Install and configure API Explorer:
```csharp
builder.Services.AddApiExplorer();
```

5. At runtime, API version passed in querystring, header, or default set controls which version is used.

## Interview Quick Reference

| Concept | Why It's Important | Key Takeaway for Interviews |
| :-- | :-- | :-- |
| **Multiple OpenAPI Specs** | Generating separate OpenAPI documents per API version clarifies usage, helps client generation, and supports backward compatibility. | "I configure my minimal APIs to produce multiple OpenAPI specs, one for each API version, so consumers can accurately target the version they need." |
| **Grouping Endpoints by Version** | Assigning endpoints to version groups ensures documentation and version routing are consistent and clear. | "Using `.WithGroupName()` I group endpoints by version, allowing clean separation of the documentation and runtime routing." |
| **Swagger UI Multiple Versions** | Supporting multiple versions in Swagger UI provides a seamless developer experience in navigating and testing different API versions. | "I configure Swagger UI to list all available API versions, so developers can easily select and interact with each version's endpoints." |
| **Runtime Version Selection** | Allowing clients to specify the version through query strings or headers supports flexible version negotiation at runtime. | "My API accepts version info in query parameters or headers, ensuring clients can select the desired API version dynamically." |
| **Validation and Documentation Synchronization** | Using API Explorer alongside versioning ensures that OpenAPI docs reflect the true capabilities and versions of the API. | "I integrate API Explorer to sync runtime API behavior and OpenAPI documentation, providing accurate and reliable API specs." |

