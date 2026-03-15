using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinalRound.Contracts.Api.Identity.Request;
using FinalRound.Contracts.Api.Identity.Response;
using FinalRound.Identity.Api.Data;
using FinalRound.Identity.Api.Domain.Models;
using FinalRound.Identity.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalRound.Identity.Api.Services;

public sealed class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly ITokenService _tokenService;

    public AuthService(
        UserManager<AppUser> userManager,
        ApplicationDbContext dbContext,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var message = string.Join(" | ", result.Errors.Select(x => x.Description));
            throw new InvalidOperationException(message);
        }

        await _userManager.AddToRoleAsync(user, "User");

        return await _tokenService.GenerateTokensAsync(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        return await _tokenService.GenerateTokensAsync(user);
    }

    public async Task<AuthResponse> RefreshAsync(RefreshRequest request)
    {
        var refreshToken = await _dbContext.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

        if (refreshToken is null || !refreshToken.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        refreshToken.RevokedAtUtc = DateTime.UtcNow;

        var response = await _tokenService.GenerateTokensAsync(refreshToken.User);

        refreshToken.ReplacedByToken = response.RefreshToken;
        await _dbContext.SaveChangesAsync();

        await transaction.CommitAsync();

        return response;
    }

    public async Task LogoutAsync(Guid userId, LogoutRequest request)
    {
        var refreshToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken && x.UserId == userId);

        if (refreshToken is null)
        {
            return;
        }

        if (refreshToken.RevokedAtUtc is null)
        {
            refreshToken.RevokedAtUtc = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<MeResponse> GetMeAsync(ClaimsPrincipal principal)
    {
        var userIdValue =
            principal.FindFirstValue(ClaimTypes.NameIdentifier) ??
            principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user.");
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        return new MeResponse
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles.ToArray()
        };
    }
}