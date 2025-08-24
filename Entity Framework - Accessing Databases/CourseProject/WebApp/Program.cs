using Polly;
using WebApp.Filters;
using WebApp.MessageHandlers;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    options.Filters.Add<WriteToConsoleResourceFilter>();
});

builder.Services.AddSingleton<IDepartmentsRepository, DepartmentsRepository>();
builder.Services.AddSingleton<IEmployeesRepository, EmployeesRepository>();

builder.Services.AddTransient<IDepartmentsApiRepository, DepartmentsApiRepository>();
builder.Services.AddTransient<ValidateApiHeaderHandler>();

builder.Services.AddHttpClient("ApiEndpoints", (HttpClient client) =>
{
    client.BaseAddress = new Uri("http://localhost:5065/");
})
//.AddHttpMessageHandler<ValidateApiHeaderHandler>()
.AddTransientHttpErrorPolicy(policy =>
{
    return policy.WaitAndRetryAsync(new[]
    {
        TimeSpan.FromMilliseconds(100),
        TimeSpan.FromMilliseconds(200),
        TimeSpan.FromMilliseconds(300),
    });
});

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
        ctx.Context.Response.Headers.Append("Expires", DateTime.UtcNow.AddMinutes(10).ToString());
    }
});

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
        );

    endpoints.MapRazorPages();
});

app.Run();
