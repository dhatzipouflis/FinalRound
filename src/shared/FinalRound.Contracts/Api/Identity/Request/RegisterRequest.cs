using System.ComponentModel.DataAnnotations;
using FinalRound.Contracts.Constants;

namespace FinalRound.Contracts.Api.Identity.Request;

public sealed class RegisterRequest
{
    [Required(ErrorMessage = ValidationErrors.EmailRequired)]
    [EmailAddress(ErrorMessage = ValidationErrors.InvalidEmail)]
    public required string Email { get; set; } = null!;

    [Required(ErrorMessage = ValidationErrors.PasswordRequired)]
    [MinLength(8, ErrorMessage = ValidationErrors.PasswordMinLength)]
    public required string Password { get; set; } = null!;

    [Required(ErrorMessage = ValidationErrors.FirstNameRequired)]
    public required string FirstName { get; set; } = null!;

    [Required(ErrorMessage = ValidationErrors.LastNameRequired)]
    public required string LastName { get; set; } = null!;
}