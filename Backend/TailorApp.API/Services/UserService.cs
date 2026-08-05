using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TailorApp.API.DTOs;
using TailorApp.API.Models;

namespace TailorApp.API.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
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
        return result;
    }

    public async Task<bool> EmailExistsAsync(string email) =>
        await _userManager.FindByEmailAsync(email) != null;

    public async Task<(bool success, IEnumerable<string> errors)> RegisterAsync(RegisterDto dto)
    {
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
            return (false, result.Errors.Select(e => e.Description));

        var validRoles = new[] { "Admin", "Staff" };
        var role = validRoles.Contains(dto.Role) ? dto.Role : "Staff";
        await _userManager.AddToRoleAsync(user, role);

        return (true, []);
    }

    public async Task<bool> UpdateAsync(string id, UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return false;

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.ShopId = dto.ShopId;

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        var role = new[] { "Admin", "Staff" }.Contains(dto.Role) ? dto.Role : "Staff";
        await _userManager.AddToRoleAsync(user, role);

        await _userManager.UpdateAsync(user);
        return true;
    }
}
