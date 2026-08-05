using TailorApp.API.DTOs;

namespace TailorApp.API.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<bool> EmailExistsAsync(string email);
    Task<(bool success, IEnumerable<string> errors)> RegisterAsync(RegisterDto dto);
    Task<bool> UpdateAsync(string id, UpdateUserDto dto);
}
