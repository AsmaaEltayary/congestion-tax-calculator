
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace congestion_tax_calculator.Web.Middleware
{
    public class GlobalExceptionHandling(ILogger<GlobalExceptionHandling> logger) : IMiddleware
    {


        

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails details = new()
                {

                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "INTERNAL SERVER ERROR -MW",
                    Type = "Server Error",
                    Detail = ex.Message
                };

                string jsonDetails = JsonSerializer.Serialize(details);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(jsonDetails);
            }
        }
    }
}
