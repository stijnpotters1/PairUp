namespace PairUpShared.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            await HandleExceptionAsync(context, error);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception error)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        string message = "Internal Server Error.";

        switch (error)
        {
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = notFoundException.Message ?? "Requested resource was not found.";
                break;
            case AlreadyExistsException alreadyExistsException:
                statusCode = HttpStatusCode.Conflict;
                message = alreadyExistsException.Message ?? "Requested resource already exists.";
                break;
            case BadRequestException badRequestException:
                statusCode = HttpStatusCode.BadRequest;
                message = badRequestException.Message ?? "Invalid request data.";
                break;
            case UnauthorizedException unauthorizedException:
                statusCode = HttpStatusCode.Forbidden;
                message = unauthorizedException.Message;
                break;
            default:
                Console.WriteLine(error.Message);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(new { statusCode = (int)statusCode, message });
        await context.Response.WriteAsync(result);
    }
}