namespace LearnMiddleware.MiddlewareComponents
{
    public class ExceptionMiddleware: IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Call the next middleware in the pipeline
                await next(context);
            }
            catch (Exception ex)
            {
                // Log the exception here (optional)
                // Write a user-friendly error page
                //context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("<h2>Error encountered</h2>");
                await context.Response.WriteAsync($"<p>{ex.Message}</p>");
                if (ex.InnerException != null)
                {
                    await context.Response.WriteAsync($"<p>Inner error: {ex.InnerException.Message}</p>");
                }
            }
        }
    }
}
