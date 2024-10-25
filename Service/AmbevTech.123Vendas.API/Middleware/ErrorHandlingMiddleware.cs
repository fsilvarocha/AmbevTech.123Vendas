using AmbevTech._123Vendas.Domain.Exception;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;
using System.Text.Json;

namespace AmbevTech._123Vendas.API.Middleware;

public class ErrorHandlingMiddleware : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        Log.Error(exception, "Error");

        string result = JsonSerializer.Serialize(new { error = exception?.Message });

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await httpContext.Response.WriteAsync(result);

        return true;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, BusinessException exception, CancellationToken cancellationToken)
    {
        Log.Error(exception.Message);

        string result = JsonSerializer.Serialize(new { error = exception?.Message });

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await httpContext.Response.WriteAsync(result);

        return true;
    }
}
