using FinalRound.Contracts.Api.Identity.Response;
using FinalRound.Identity.Api.Domain.Models;

namespace FinalRound.Identity.Api.Services.Interfaces;

public interface ITokenService
{
    Task<AuthResponse> GenerateTokensAsync(AppUser user, CancellationToken cancellationToken = default);
}