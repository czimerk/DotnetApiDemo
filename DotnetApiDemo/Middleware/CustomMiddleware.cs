namespace DotnetApiDemo.Middleware
{
    public class CustomMiddleware
    {
        readonly RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value.Contains("ping"))
            {
                await context.Response.WriteAsync("pong");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
