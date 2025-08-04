
namespace LearnMiddleware.MiddlewareComponents
{
    public class MyCustomMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            
            await context.Response.WriteAsync("My custom middleware: Before calling the next middleware\n");
            // Call the next middleware in the pipeline and pass the HttpContext object
            await next(context);

            await context.Response.WriteAsync("My custom middleware: After calling the next middleware\n");
        }
    }
}
