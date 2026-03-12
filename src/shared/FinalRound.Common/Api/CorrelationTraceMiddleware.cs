using Microsoft.AspNetCore.Http;

namespace FinalRound.Common.Api;

public sealed class CorrelationTraceMiddleware
{
    public const string CorrelationHeader = "X-Correlation-Id";
    public const string TraceHeader = "X-Trace-Id";

    private readonly RequestDelegate _next;

    public CorrelationTraceMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx)
    {
        var correlationId =
            ctx.Request.Headers.TryGetValue(CorrelationHeader, out var v) && !string.IsNullOrWhiteSpace(v)
                ? v.ToString()
                : Guid.NewGuid().ToString("N");

        ctx.Request.Headers[CorrelationHeader] = correlationId;

        ctx.Response.OnStarting(() =>
        {
            ctx.Response.Headers[CorrelationHeader] = correlationId;
            ctx.Response.Headers[TraceHeader] = ctx.TraceIdentifier;
            ctx.Response.Headers["Access-Control-Expose-Headers"] = $"{CorrelationHeader}, {TraceHeader}";
            return Task.CompletedTask;
        });

        await _next(ctx);
    }
}
