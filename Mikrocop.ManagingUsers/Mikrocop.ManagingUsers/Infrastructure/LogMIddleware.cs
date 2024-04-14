using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace Mikrocop.ManagingUsers.Infrastructure
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var logMessage = new
                {
                    LogLevel = "Info",
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ClientIP = context.Connection.RemoteIpAddress?.ToString(),
                    ClientName = context.Request.Headers["User-Agent"],
                    HostName = Environment.MachineName,
                    MethodName = context.Request.Method,
                    RequestParameters = context.Request.QueryString,
                    Message = "API call"
                };

                string logEntry = JsonConvert.SerializeObject(logMessage);

                Log.Logger.Information(logEntry);

                await _next(context);
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json; charset=utf-8";
            }
        }
    }
}
