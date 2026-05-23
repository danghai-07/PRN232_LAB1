using PRN232.LMS.API.Common;
using PRN232.LMS.Services.Exceptions;
using System.Net;
using System.Text.Json;

namespace PRN232.LMS.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                throw;
            }

            context.Response.ContentType =
                "application/json";

            var response =
                new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                    Errors = null
                };

            switch (ex)
            {
                case BadRequestException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.BadRequest;
                    break;

                case NotFoundException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.NotFound;
                    break;

                case ConflictException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.Conflict;
                    break;

                default:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var json =
                JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}