
namespace WebApp.MessageHandlers
{
    public class ValidateApiHeaderHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(request.Headers.Contains("X-Api-Key"))
            {
                var apiKey = request.Headers.GetValues("X-Api-Key").FirstOrDefault();
                if (string.IsNullOrEmpty(apiKey) || apiKey != "YourExpectedApiKey")
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("Invalid API Key")
                    };
                }
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("API Key is required")
                };
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
