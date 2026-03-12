using System.Text.Json;
using System.Text.Json.Serialization;
using FinalRound.Contracts.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace FinalRound.Common.Api;

public sealed class ApiExceptionMiddleware
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = null,
        DictionaryKeyPolicy = null,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly RequestDelegate _next;
    private readonly IOptionsMonitor<ErrorHandlingOptions> _opts;

    public ApiExceptionMiddleware(RequestDelegate next, IOptionsMonitor<ErrorHandlingOptions> opts)
    {
        _next = next;
        _opts = opts;
    }

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            var correlationId = ctx.Request.Headers.TryGetValue(CorrelationTraceMiddleware.CorrelationHeader, out var v)
                ? v.ToString()
                : Guid.NewGuid().ToString("N");

            var message = _opts.CurrentValue.IncludeDetailedMessages ? ex.Message : "Unexpected error";

            var body = ApiResponse<object>.Fail(
                [new ApiError("unhandled_exception", message)],
                ctx.TraceIdentifier,
                correlationId);

            ctx.Response.StatusCode = StatusCodes.Status200OK;
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(body, JsonOpts), ctx.RequestAborted);
        }
    }
}
