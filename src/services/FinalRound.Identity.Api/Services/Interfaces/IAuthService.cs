using System.Security.Claims;
using FinalRound.Contracts.Api.Identity.Request;
using FinalRound.Contracts.Api.Identity.Response;

namespace FinalRound.Identity.Api.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshAsync(RefreshRequest request);
    Task LogoutAsync(Guid userId, LogoutRequest request);
    Task<MeResponse> GetMeAsync(ClaimsPrincipal principal);
}