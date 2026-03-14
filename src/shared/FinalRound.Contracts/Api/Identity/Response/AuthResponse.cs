namespace FinalRound.Contracts.Api.Identity.Response;

public sealed class AuthResponse
{
    public required string AccessToken { get; set; }
    public required DateTime AccessTokenExpiresAtUtc { get; set; }
    public required string RefreshToken { get; set; }
    public string TokenType { get; set; } = "Bearer";
}