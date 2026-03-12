using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Dapr.Client;
using FinalRound.Common.Api;
using FinalRound.Contracts.Api;
using Microsoft.Extensions.Options;

namespace FinalRound.Edge.Api.Invocation;

public sealed class ServiceInvocationClient : IServiceInvocationClient
{
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    private readonly DaprClient _daprClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ErrorHandlingOptions _errorOptions;

    public ServiceInvocationClient(
        DaprClient daprClient,
        IHttpContextAccessor httpContextAccessor,
        IOptions<ErrorHandlingOptions> errorOptions)
    {
        _daprClient = daprClient;
        _httpContextAccessor = httpContextAccessor;
        _errorOptions = errorOptions.Value;
    }

    public Task<ApiResponse<TResponse>> InvokeAsync<TResponse>(ResolvedServiceInvocation route, CancellationToken ct = default)
        => SendAsync<TResponse>(route, null, ct);

    public Task<ApiResponse<TResponse>> InvokeAsync<TRequest, TResponse>(ResolvedServiceInvocation route, TRequest request, CancellationToken ct = default)
        => SendAsync<TResponse>(route, request, ct);

    private async Task<ApiResponse<TResponse>> SendAsync<TResponse>(ResolvedServiceInvocation route, object? requestBody, CancellationToken ct)
    {
        var ctx = _httpContextAccessor.HttpContext!;
        var traceId = ctx.TraceIdentifier;
        var correlationId = ctx.Request.Headers.TryGetValue(CorrelationTraceMiddleware.CorrelationHeader, out var cv)
            ? cv.ToString()
            : Guid.NewGuid().ToString("N");

        try
        {
            var request = _daprClient.CreateInvokeMethodRequest(route.Method, route.AppId, route.MethodRoute);

            if (requestBody is not null)
                request.Content = JsonContent.Create(requestBody);

            request.Headers.TryAddWithoutValidation(CorrelationTraceMiddleware.CorrelationHeader, correlationId);

#pragma warning disable CS0618
            var response = await _daprClient.InvokeMethodWithResponseAsync(request, ct);
#pragma warning restore CS0618
            var json = await response.Content.ReadAsStringAsync(ct);

            var parsed = Try<ApiResponse<TResponse>>(json);
            if (parsed is not null)
                return parsed with { TraceId = traceId, CorrelationId = correlationId };

            var err = new ApiError("upstream_nonstandard_response", $"Upstream returned {(HttpStatusCode)response.StatusCode}");
            return ApiResponse<TResponse>.Fail([err], traceId, correlationId);
        }
        catch (Exception ex)
        {
            var msg = _errorOptions.IncludeDetailedMessages ? ex.Message : "Upstream invocation failed";
            return ApiResponse<TResponse>.Fail([new ApiError("invocation_exception", msg)], traceId, correlationId);
        }
    }

    private static T? Try<T>(string json)
    {
        try { return JsonSerializer.Deserialize<T>(json, JsonOpts); }
        catch { return default; }
    }
}
