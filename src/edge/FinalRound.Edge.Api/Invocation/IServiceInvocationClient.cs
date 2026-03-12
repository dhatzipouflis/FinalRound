using FinalRound.Contracts.Api;

namespace FinalRound.Edge.Api.Invocation;

public interface IServiceInvocationClient
{
    Task<ApiResponse<TResponse>> InvokeAsync<TResponse>(ResolvedServiceInvocation route, CancellationToken ct = default);
    Task<ApiResponse<TResponse>> InvokeAsync<TRequest, TResponse>(ResolvedServiceInvocation route, TRequest request, CancellationToken ct = default);
}
