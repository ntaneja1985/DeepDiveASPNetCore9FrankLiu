
using System.Text;

namespace LearnRouting.Results
{
    public class HtmlResult : IResult
    {
        private readonly string _htmlContent;

        public HtmlResult(string htmlContent)
        {
            _htmlContent = htmlContent;
        }

        public async Task ExecuteAsync(HttpContext httpContext)
        {
           httpContext.Response.ContentType = "text/html";
            httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_htmlContent);
            await  httpContext.Response.WriteAsync(_htmlContent);
        }
    }
}
