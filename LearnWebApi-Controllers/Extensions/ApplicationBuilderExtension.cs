using Asp.Versioning;
using Asp.Versioning.Builder;
using System.Runtime.CompilerServices;

namespace LearnWebApi_Controllers.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static void MapApplicationBuilderExtensions(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            app.MapGet("/hello", () => "Hello World!")
                .WithName("HelloWorldv1")
                .WithSummary("Returns a simple hello world message")
                .WithTags("Greetings")
                .WithApiVersionSet(apiVersionSet)
                .MapToApiVersion(new ApiVersion(1,0))
                .WithGroupName("v1");

            app.MapGet("/hello", () => "Hello World FROM V2!")
                .WithName("HelloWorldv2")
                .WithSummary("Returns a simple hello world message v2")
                .WithTags("Greetings")
                .WithApiVersionSet(apiVersionSet)
                .MapToApiVersion(new ApiVersion(2, 0))
                .WithGroupName("v2");
        }
    }
}
