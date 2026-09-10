using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorApp.API.DTOs;
using TailorApp.API.Services;

namespace TailorApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto dto)
    {
        if (await _userService.EmailExistsAsync(dto.Email))
            return BadRequest("Email already exists.");

        var (success, errors) = await _userService.RegisterAsync(dto);
        if (!success) return BadRequest(errors);

        return StatusCode(201, "User registered successfully.");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(string id, UpdateUserDto dto)
    {
        var found = await _userService.UpdateAsync(id, dto);
        if (!found) return NotFound();
        return Ok("User updated successfully.");
    }
}
