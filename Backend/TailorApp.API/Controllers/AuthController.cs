using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TailorApp.API.DTOs;
using TailorApp.API.Models;
using TailorApp.API.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;

namespace TailorApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly TokenService _tokenService;
    private readonly string _frontendBaseUrl;

    public AuthController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        TokenService tokenService,
        IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _frontendBaseUrl = config["App:FrontendBaseUrl"] ?? "http://localhost:4200";
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !user.IsActive)
            return Unauthorized("Invalid credentials.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            return Unauthorized("Invalid credentials.");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Staff";
        var (token, expiry) = _tokenService.CreateToken(user, role);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = role,
            ShopId = user.ShopId,
            Expiry = expiry
        });
    }

    [HttpGet("google")]
    public IActionResult GoogleLogin()
    {
        var redirectUrl = Url.Action(nameof(GoogleCallback), "Auth");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(
            GoogleDefaults.AuthenticationScheme, redirectUrl);
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return Redirect($"{_frontendBaseUrl}/login?error=google_failed");

        var email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
        var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "";
        var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "";

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
                return Redirect($"{_frontendBaseUrl}/login?error=create_failed");

            await _userManager.AddToRoleAsync(user, "Staff");
        }

        if (!user.IsActive)
            return Redirect($"{_frontendBaseUrl}/login?error=account_inactive");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Staff";
        var (token, expiry) = _tokenService.CreateToken(user, role);

        // Token in the URL fragment — not sent to servers, not in access logs
        var fragment = $"token={Uri.EscapeDataString(token)}" +
            $"&email={Uri.EscapeDataString(email)}" +
            $"&firstName={Uri.EscapeDataString(firstName)}" +
            $"&lastName={Uri.EscapeDataString(lastName)}" +
            $"&role={Uri.EscapeDataString(role)}" +
            $"&shopId={user.ShopId}" +
            $"&expiry={Uri.EscapeDataString(expiry.ToString("O"))}";

        return Redirect($"{_frontendBaseUrl}/auth/callback#{fragment}");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto dto)
    {
        if (await _userManager.FindByEmailAsync(dto.Email) != null)
            return BadRequest("Email already exists.");

        var user = new AppUser
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email,
            ShopId = dto.ShopId,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));

        var validRoles = new[] { "Admin", "Staff" };
        var role = validRoles.Contains(dto.Role) ? dto.Role : "Staff";
        await _userManager.AddToRoleAsync(user, role);

        return StatusCode(201, "User registered successfully.");
    }

    [Authorize]
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _userManager.Users.Where(u => u.IsActive).ToListAsync();
        var result = new List<UserDto>();
        foreach (var u in users)
        {
            var roles = await _userManager.GetRolesAsync(u);
            result.Add(new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email!,
                Role = roles.FirstOrDefault() ?? "Staff",
                ShopId = u.ShopId,
                IsActive = u.IsActive,
                CreatedDate = u.CreatedDate
            });
        }

        return Ok(result);
    }


    [Authorize]
    [HttpPut("users/{id}")]
    public async Task<ActionResult> UpdateUser(string id, UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.ShopId = dto.ShopId;

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        var role = new[] { "Admin", "Staff" }.Contains(dto.Role) ? dto.Role : "Staff";
        await _userManager.AddToRoleAsync(user, role);

        await _userManager.UpdateAsync(user);
        return Ok("User updated successfully.");
    }
}
