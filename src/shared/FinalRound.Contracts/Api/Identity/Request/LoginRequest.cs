using System.ComponentModel.DataAnnotations;
using FinalRound.Contracts.Constants;

namespace FinalRound.Contracts.Api.Identity.Request;

public sealed class LoginRequest
{
    [Required(ErrorMessage = ValidationErrors.EmailRequired)]
    [EmailAddress(ErrorMessage = ValidationErrors.InvalidEmail)]
    public required string Email { get; set; } = null!;

    [Required(ErrorMessage = ValidationErrors.PasswordRequired)]
    public required string Password { get; set; } = null!;
}