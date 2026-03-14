using System.ComponentModel.DataAnnotations;
using FinalRound.Contracts.Constants;

namespace FinalRound.Contracts.Api.Identity.Request;

public sealed class RefreshRequest
{
    [Required(ErrorMessage = ValidationErrors.RefreshTokenRequired)]
    public required string RefreshToken { get; set; } = null!;
}