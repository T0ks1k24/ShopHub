using System.Net;
using Newtonsoft.Json;
using ProductManagementApi.Middleware.Exceptions;


namespace ProductManagementApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (UnauthorizedAccessException)
            {
                //401
                await HandleErrorAsync(httpContext, HttpStatusCode.Unauthorized, "You are not authorized.");
            }
            catch (ForbiddenAccessException)
            {
                //403
                await HandleErrorAsync(httpContext, HttpStatusCode.Forbidden, "Access to this resource is forbidden.");
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(httpContext, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        private async Task HandleErrorAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new { message = message };
            var errorJson = JsonConvert.SerializeObject(errorResponse);

            await context.Response.WriteAsync(errorJson);
        }
    }
}
