using Microsoft.AspNetCore.Builder;

namespace FinalRound.Common.Api;

public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder UseApiExceptionHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ApiExceptionMiddleware>();
}
