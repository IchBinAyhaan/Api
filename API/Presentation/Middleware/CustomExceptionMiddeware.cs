using Business.Wrappers;
using Common.Exceptions;
using System.Text.Json;

namespace Presentation.Middleware
{
    public class CustomExceptionMiddeware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<CustomExceptionMiddeware> _logger;
        public CustomExceptionMiddeware(RequestDelegate requestDelegate, ILogger<CustomExceptionMiddeware> logger)
        {
            next = requestDelegate;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                var response = new Response();
                switch (e)
                {
                    case ValidationException ex:
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        response.Errors = ex.Errors;
                        break;
                    case NotFoundException ex:
                        context.Response.StatusCode = StatusCodes.Status404NotFound; response.Errors = ex.Errors;
                        response.Errors = ex.Errors;
                        break;
                    case UnauthorizedException ex:
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized; response.Errors = ex.Errors;
                            response.Errors = ex.Errors;
                        break;
                    default:
                        _logger.LogError($"Message {e.Message} Inner Exception: {e.InnerException}");

                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        response.Message = "Xeta bas verdi";
                        break;
                }
                await context.Response.WriteAsJsonAsync(response);

            }
        }
    }
}
