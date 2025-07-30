namespace WebAppTest.Exceptions;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (HttpException ex)
        {
            context.Response.StatusCode = (int)ex.StatusCode;
            context.Response.ContentType = "application/json";

            var response = new { message = ex.Message };
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new { message = "Внутренняя ошибка сервера" };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
