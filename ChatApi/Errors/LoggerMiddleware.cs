using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ChatApi.Errors
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggerMiddleware> _logger;

        public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            try
            {
                return _next(httpContext);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Internal error proizashlo");
                httpContext.Response.StatusCode = 500;
                return httpContext.Response.WriteAsJsonAsync(new { Error = "Xattooo" });
            }

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggerMiddleware>();
        }
    }
}
