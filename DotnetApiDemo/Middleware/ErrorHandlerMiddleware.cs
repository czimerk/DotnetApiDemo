using nj = Newtonsoft.Json;

namespace DotnetApiDemo.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var message = ex.Message;
                switch (ex)
                {
                    case BadHttpRequestException valEx:
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        message = nj.JsonConvert.SerializeObject(valEx.Message);
                        break;
                    case InvalidOperationException valEx:
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        message = nj.JsonConvert.SerializeObject(valEx.Message);
                        break;
                    case UnauthorizedAccessException valEx:
                        response.StatusCode = StatusCodes.Status401Unauthorized;
                        message = nj.JsonConvert.SerializeObject(valEx.Message);
                        break;
                    default:
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        _logger.LogError(ex, $"[{nameof(ErrorHandlerMiddleware)}]: Unhandled exception happened.");
                        break;
                }

                await response.WriteAsync(message);
            }
        }
    }
}
