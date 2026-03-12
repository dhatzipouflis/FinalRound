namespace FinalRound.Edge.Api.Invocation;

public interface IServiceInvocationResolver
{
    ResolvedServiceInvocation Resolve(string key);
}
