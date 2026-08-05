using Microsoft.AspNetCore.Identity;
using TailorApp.API.DTOs;
using TailorApp.API.Models;

namespace TailorApp.API.Services;

public record GoogleUserResult(string? Error, AppUser? User, string Role, string Token, DateTime Expiry);

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly TokenService _tokenService;

    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        TokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !user.IsActive) return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded) return null;

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Staff";
        var (token, expiry) = _tokenService.CreateToken(user, role);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = role,
            ShopId = user.ShopId,
            Expiry = expiry
        };
    }

    public async Task<GoogleUserResult> HandleGoogleUserAsync(string email, string firstName, string lastName)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new AppUser
            {
                Email = email,
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                ShopId = 1,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
                return new GoogleUserResult("create_failed", null, "", "", default);

            await _userManager.AddToRoleAsync(user, "Staff");
        }

        if (!user.IsActive)
            return new GoogleUserResult("account_inactive", null, "", "", default);

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Staff";
        var (token, expiry) = _tokenService.CreateToken(user, role);

        return new GoogleUserResult(null, user, role, token, expiry);
    }
}
