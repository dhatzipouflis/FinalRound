using Microsoft.Extensions.Options;

namespace FinalRound.Edge.Api.Invocation;

public sealed class ServiceInvocationResolver : IServiceInvocationResolver
{
    private readonly IOptionsMonitor<ServiceInvocationOptions> _options;

    public ServiceInvocationResolver(IOptionsMonitor<ServiceInvocationOptions> options) => _options = options;

    public ResolvedServiceInvocation Resolve(string key)
    {
        if (!_options.CurrentValue.Routes.TryGetValue(key, out var route))
            throw new InvalidOperationException($"Service invocation route '{key}' was not found.");

        var method = route.Protocol.ToUpperInvariant() switch
        {
            "GET" => HttpMethod.Get,
            "POST" => HttpMethod.Post,
            "PUT" => HttpMethod.Put,
            "DELETE" => HttpMethod.Delete,
            "PATCH" => HttpMethod.Patch,
            _ => throw new InvalidOperationException($"Invalid protocol '{route.Protocol}' for route '{key}'.")
        };

        return new ResolvedServiceInvocation(key, route.RouteToAppId, route.MethodRoute.TrimStart('/'), method);
    }
}
