using FinalRound.Contracts.Api;
using Microsoft.AspNetCore.Mvc;

namespace FinalRound.Common.Api;

public static class ControllerApiExtensions
{
    public static IActionResult ApiOk<T>(this ControllerBase c, T data)
    {
        var correlationId = c.Request.Headers.TryGetValue(CorrelationTraceMiddleware.CorrelationHeader, out var v)
            ? v.ToString()
            : Guid.NewGuid().ToString("N");

        return c.Ok(ApiResponse<T>.Ok(data, c.HttpContext.TraceIdentifier, correlationId));
    }
}
