using Asp.Versioning;
using Asp.Versioning.Builder;
using LearnWebApi_Controllers.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Corrected OpenAPI Configuration

// Corrected OpenAPI Configuration in Program.cs

builder.Services.AddOpenApi("v1", options =>
{
    // ASSIGN the lambda to the ShouldInclude property
    options.ShouldInclude = apiDesc => apiDesc.GroupName == "v1";
});

builder.Services.AddOpenApi("v2", options =>
{
    // ASSIGN the lambda to the ShouldInclude property
    options.ShouldInclude = apiDesc => apiDesc.GroupName == "v2";
});



builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; // Enable reporting of API versions
    options.AssumeDefaultVersionWhenUnspecified = true; // Use default version if not specified
    options.DefaultApiVersion = new ApiVersion(1, 0); // Set default API version
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"), // Read version from query string
        new HeaderApiVersionReader("X-API-Version") // Read version from custom header
    );
}).AddApiExplorer().AddMvc();



var app = builder.Build();

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .HasApiVersion(new ApiVersion(2))
    .ReportApiVersions()
    .Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi(); // Enable OpenAPI in development

    // Middleware to serve the Swagger UI, pointing to the JSON file
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "API Version 1");
        c.SwaggerEndpoint("/openapi/v2.json", "API Version 2");
    });
}
else
{
    app.UseExceptionHandler("/error"); // Custom error handling in production
}
app.UseAuthorization();
app.MapApplicationBuilderExtensions(apiVersionSet);

app.MapControllers();

app.Run();
