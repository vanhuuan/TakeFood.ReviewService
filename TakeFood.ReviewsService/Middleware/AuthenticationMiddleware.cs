using Microsoft.Extensions.Primitives;

namespace TakeFood.ReviewsService.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var id = context.Request.Headers.TryGetValue("Uid", out StringValues sv);
            context.Items["Id"] = sv.ToString();
            await _next(context);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
