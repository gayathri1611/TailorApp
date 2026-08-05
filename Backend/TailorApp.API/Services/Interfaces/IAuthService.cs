using TailorApp.API.DTOs;

namespace TailorApp.API.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    Task<GoogleUserResult> HandleGoogleUserAsync(string email, string firstName, string lastName);
}
