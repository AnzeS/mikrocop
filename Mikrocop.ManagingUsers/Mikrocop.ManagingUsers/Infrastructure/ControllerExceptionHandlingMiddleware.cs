using Serilog;
using System.Net;

namespace Mikrocop.ManagingUsers.Midlewares
{
    public class ControllerExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ControllerExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}: {ex.StackTrace}");
                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ContentType = "application/json; charset=utf-8";

                switch (ex)
                {
                    case ArgumentException:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public static class ControllerExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseControllerExceptionHandlingMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ControllerExceptionHandlingMiddleware>();
        }
    }
}