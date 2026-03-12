namespace FinalRound.Edge.Api.Invocation;

public sealed record ResolvedServiceInvocation(string Key, string AppId, string MethodRoute, HttpMethod Method);
