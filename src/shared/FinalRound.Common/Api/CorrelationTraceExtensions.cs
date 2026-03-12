using Microsoft.AspNetCore.Builder;

namespace FinalRound.Common.Api;

public static class CorrelationTraceExtensions
{
    public static IApplicationBuilder UseCorrelationAndTraceHeaders(this IApplicationBuilder app)
        => app.UseMiddleware<CorrelationTraceMiddleware>();
}
