using Polly;
using WebApp.Filters;
using WebApp.MessageHandlers;
using WebApp.Model;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders();
//builder.Logging.AddJsonConsole(options =>
//{
//    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss";
//});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    options.Filters.Add<WriteToConsoleResourceFilter>();
});

//builder.Services.AddSingleton<IDepartmentsRepository, DepartmentsRepository>();
//builder.Services.AddSingleton<IEmployeesRepository, EmployeesRepository>();

builder.Services.AddTransient<IDepartmentsApiRepository, DepartmentsApiRepository>();
builder.Services.AddTransient<IEmployeesApiRepository, EmployeesApiRepository>();
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

builder.Services.AddAuthentication("CookieScheme").AddCookie("CookieScheme", options =>
{
    options.Cookie.Name = "CookieScheme";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
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
