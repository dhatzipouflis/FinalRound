using FinalRound.Contracts.Api;
using FinalRound.Edge.Api.Invocation;

namespace FinalRound.Edge.Api.Services;

public abstract class ServiceInvocationServiceBase
{
    private readonly IServiceInvocationResolver _resolver;
    private readonly IServiceInvocationClient _client;

    protected ServiceInvocationServiceBase(IServiceInvocationResolver resolver, IServiceInvocationClient client)
    {
        _resolver = resolver;
        _client = client;
    }

    protected Task<ApiResponse<TResponse>> InvokeAsync<TResponse>(string routeKey, CancellationToken ct = default)
    {
        var route = _resolver.Resolve(routeKey);
        return _client.InvokeAsync<TResponse>(route, ct);
    }

    protected Task<ApiResponse<TResponse>> InvokeAsync<TRequest, TResponse>(string routeKey, TRequest request, CancellationToken ct = default)
    {
        var route = _resolver.Resolve(routeKey);
        return _client.InvokeAsync<TRequest, TResponse>(route, request, ct);
    }
}
