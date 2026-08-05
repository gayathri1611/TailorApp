using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorApp.API.DTOs;
using TailorApp.API.Services;

namespace TailorApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NameValuesController : ControllerBase
{
    private readonly INameValueService _nameValueService;

    public NameValuesController(INameValueService nameValueService)
    {
        _nameValueService = nameValueService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NameValueDto>>> GetAll()
    {
        return Ok(await _nameValueService.GetAllAsync());
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<NameValueDto>>> GetByCategory(string category)
    {
        return Ok(await _nameValueService.GetByCategoryAsync(category));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> Create(CreateNameValueDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Category)) return BadRequest("Category is required.");
        if (string.IsNullOrWhiteSpace(dto.Value)) return BadRequest("Value is required.");

        var (error, item) = await _nameValueService.CreateAsync(dto);
        if (error != null) return BadRequest(error);
        return Ok(item);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateNameValueDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Value)) return BadRequest("Value is required.");

        var (error, found) = await _nameValueService.UpdateAsync(id, dto);
        if (!found) return NotFound();
        if (error != null) return BadRequest(error);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _nameValueService.DeleteAsync(id)) return NotFound();
        return NoContent();
    }
}
