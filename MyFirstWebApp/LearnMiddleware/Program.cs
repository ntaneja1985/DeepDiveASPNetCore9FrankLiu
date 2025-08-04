using LearnMiddleware.MiddlewareComponents;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<MyCustomMiddleware>(); // Register the custom middleware
builder.Services.AddTransient<ExceptionMiddleware>(); // Register the exception middleware

var app = builder.Build();

// Configure the HTTP request middleware pipeline

//app.UseHttpsRedirection();

//app.UseRouting();

//app.UseAuthentication();

//app.UseAuthorization();

// Middleware to handle exceptions globally
app.UseMiddleware<ExceptionMiddleware>();

//Middleware #1
app.Use(async (HttpContext context,RequestDelegate next) =>
{
    context.Response.ContentType = "text/html";
    // This is the first middleware in the pipeline before the next middleware is called
    await context.Response.WriteAsync("Middleware # 1: Before calling the next middleware\n");
    // Call the next middleware in the pipeline and pass the HttpContext object
    await next(context);
    // Note: that the context object may be modified by the next middleware
    // After calling the next middleware, you can perform additional actions
    await context.Response.WriteAsync("Middleware # 1: After calling the next middleware\n");
});

app.UseMiddleware<MyCustomMiddleware>(); // Use the custom middleware

#region
//app.UseWhen((context) => //Specify the condition for this middleware to run
//{
//    return  context.Request.Path.StartsWithSegments("/employees") &&
//            context.Request.Query.ContainsKey("id"); //Check the query string for the key "id"

//}, (appBuilder) =>
//{
//    appBuilder.Use(async (HttpContext context, RequestDelegate next) =>
//    {
//        await context.Response.WriteAsync("Middleware # 5: Before calling the next middleware\n");
//        await next(context);
//        await context.Response.WriteAsync("Middleware # 5: After calling the next middleware\n");
//    });

//    appBuilder.Use(async (HttpContext context, RequestDelegate next) =>
//    {

//        await context.Response.WriteAsync("Middleware # 6: Before calling the next middleware\n");
//        await next(context);
//        await context.Response.WriteAsync("Middleware # 6: After calling the next middleware\n");
//    });
//});


//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Middleware #2: Processed. \r\n");
//});

#endregion

//Middleware #2
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Middleware # 2: Before calling the next middleware\n");
    // Call the next middleware in the pipeline and pass the HttpContext object
    await next(context);
    // Note: that the context object may be modified by the next middleware
    // After calling the next middleware, you can perform additional actions
    await context.Response.WriteAsync("Middleware # 2: After calling the next middleware\n");
});


//Middleware #3
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    throw new Exception("An error occurred in Middleware #3"); // Simulate an exception

    await context.Response.WriteAsync("Middleware # 3: Before calling the next middleware\n");
    // Call the next middleware in the pipeline and pass the HttpContext object
    await next(context);
    // Note: that the context object may be modified by the next middleware
    // After calling the next middleware, you can perform additional actions
    await context.Response.WriteAsync("Middleware # 3: After calling the next middleware\n");
});

// Run the Kestrel server
app.Run();


