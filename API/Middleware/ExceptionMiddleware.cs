using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate _next;
        public readonly ILogger<ExceptionMiddleware> _logger;
        public readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        // Middleware recognise "InvokeAsync" not other names
        public async Task InvokeAsync (HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // show the error in the terminal
                _logger.LogError(ex, ex.Message);
                // return it to client (We won't mention this inside API as it's default, but we're not in the API, so we need to metion this.)
                context.Response.ContentType = "applicaton/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

                // return response as Json
                var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}