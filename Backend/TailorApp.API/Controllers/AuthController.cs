using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TailorApp.API.DTOs;
using TailorApp.API.Models;
using TailorApp.API.Services;

namespace TailorApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly string _frontendBaseUrl;

    public AuthController(
        IAuthService authService,
        SignInManager<AppUser> signInManager,
        IConfiguration config)
    {
        _authService = authService;
        _signInManager = signInManager;
        _frontendBaseUrl = config["App:FrontendBaseUrl"] ?? "http://localhost:4200";
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var response = await _authService.LoginAsync(dto);
        if (response == null) return Unauthorized("Invalid credentials.");
        return Ok(response);
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

        var result = await _authService.HandleGoogleUserAsync(email, firstName, lastName);
        if (result.Error != null)
            return Redirect($"{_frontendBaseUrl}/login?error={result.Error}");

        var fragment = $"token={Uri.EscapeDataString(result.Token)}" +
            $"&email={Uri.EscapeDataString(email)}" +
            $"&firstName={Uri.EscapeDataString(firstName)}" +
            $"&lastName={Uri.EscapeDataString(lastName)}" +
            $"&role={Uri.EscapeDataString(result.Role)}" +
            $"&shopId={result.User!.ShopId}" +
            $"&expiry={Uri.EscapeDataString(result.Expiry.ToString("O"))}";

        return Redirect($"{_frontendBaseUrl}/auth/callback#{fragment}");
    }
}
