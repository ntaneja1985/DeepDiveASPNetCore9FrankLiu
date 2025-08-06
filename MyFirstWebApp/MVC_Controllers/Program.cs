var builder = WebApplication.CreateBuilder(args);

//Add Xml serializer formatters for xml support
builder.Services.AddControllers().AddXmlSerializerFormatters();

var app = builder.Build();

app.UseRouting();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

app.MapControllers();
app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
