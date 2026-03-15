using FinalRound.Contracts.Api.Identity.Request;
using FinalRound.Edge.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinalRound.Edge.Api.Controllers.Identity;

[ApiController]
[Route("api/Auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        => Ok(await _identityService.RegisterAsync(request));

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
        => Ok(await _identityService.LoginAsync(request));

    [HttpPost("Refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        => Ok(await _identityService.RefreshAsync(request));

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        => Ok(await _identityService.LogoutAsync(request));

    [HttpGet("Me")]
    public async Task<IActionResult> Me()
        => Ok(await _identityService.MeAsync());
}