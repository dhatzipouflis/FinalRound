namespace FinalRound.Contracts.Api.Identity.Response;

public sealed class MeResponse
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required IReadOnlyCollection<string> Roles { get; set; }
}