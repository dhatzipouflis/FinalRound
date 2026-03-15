using FinalRound.Contracts.Api;
using FinalRound.Contracts.Api.Identity.Request;
using FinalRound.Contracts.Api.Identity.Response;

namespace FinalRound.Edge.Api.Services;

public interface IIdentityService
{
    Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse<AuthResponse>> RefreshAsync(RefreshRequest request);
    Task<ApiResponse<object>> LogoutAsync(LogoutRequest request);
    Task<ApiResponse<MeResponse>> MeAsync();
}