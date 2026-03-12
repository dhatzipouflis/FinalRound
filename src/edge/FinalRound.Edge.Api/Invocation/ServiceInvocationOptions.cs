namespace FinalRound.Edge.Api.Invocation;

public sealed class ServiceInvocationOptions
{
    public Dictionary<string, InvocationEndpoint> Routes { get; init; } = new();
}

public sealed class InvocationEndpoint
{
    public string RouteToAppId { get; init; } = default!;
    public string MethodRoute { get; init; } = default!;
    public string Protocol { get; init; } = "GET";
}
