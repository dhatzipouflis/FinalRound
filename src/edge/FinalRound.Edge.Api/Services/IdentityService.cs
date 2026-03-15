using FinalRound.Contracts.Api;
using FinalRound.Contracts.Api.Identity.Request;
using FinalRound.Contracts.Api.Identity.Response;
using FinalRound.Edge.Api.Constants;
using FinalRound.Edge.Api.Invocation;

namespace FinalRound.Edge.Api.Services;

public sealed class IdentityService : ServiceInvocationServiceBase, IIdentityService
{
    public IdentityService(IServiceInvocationResolver resolver, IServiceInvocationClient client)
        : base(resolver, client)
    {
    }

    public Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request)
        => InvokeAsync<RegisterRequest, AuthResponse>(ServiceInvocationKeys.IdentityRegister, request);

    public Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request)
        => InvokeAsync<LoginRequest, AuthResponse>(ServiceInvocationKeys.IdentityLogin, request);

    public Task<ApiResponse<AuthResponse>> RefreshAsync(RefreshRequest request)
        => InvokeAsync<RefreshRequest, AuthResponse>(ServiceInvocationKeys.IdentityRefresh, request);

    public Task<ApiResponse<object>> LogoutAsync(LogoutRequest request)
        => InvokeAsync<LogoutRequest, object>(ServiceInvocationKeys.IdentityLogout, request);

    public Task<ApiResponse<MeResponse>> MeAsync()
        => InvokeAsync<MeResponse>(ServiceInvocationKeys.IdentityMe);
}