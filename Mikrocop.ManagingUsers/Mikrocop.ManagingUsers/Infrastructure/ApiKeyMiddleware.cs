using System.Net;

namespace Mikrocop.ManagingUsers.Infrastructure
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<string> _apiKeys;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _apiKeys = configuration.GetSection("ClientKeys").Get<List<string>>()!;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var apiKey = context.Request.Headers["API-Key"];

                if (string.IsNullOrEmpty(apiKey) || !_apiKeys.Contains(apiKey!))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("Invalid API key");
                    return;
                }

                await _next(context);
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
