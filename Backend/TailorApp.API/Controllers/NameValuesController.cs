using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TailorApp.API.Data;
using TailorApp.API.DTOs;
using TailorApp.API.Models;

namespace TailorApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NameValuesController : ControllerBase
{
    private readonly TailorAppDbContext _context;

    public NameValuesController(TailorAppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NameValueDto>>> GetAll()
    {
        var items = await _context.NameValues
            .Where(n => n.IsActive)
            .OrderBy(n => n.Category)
            .ThenBy(n => n.SortOrder)
            .ThenBy(n => n.Value)
            .Select(n => new NameValueDto
            {
                NameValueId = n.NameValueId,
                Category = n.Category,
                Value = n.Value,
                Label = n.Label ?? n.Value,
                SortOrder = n.SortOrder
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<NameValueDto>>> GetByCategory(string category)
    {
        var items = await _context.NameValues
            .Where(n => n.Category == category && n.IsActive)
            .OrderBy(n => n.SortOrder)
            .ThenBy(n => n.Value)
            .Select(n => new NameValueDto
            {
                NameValueId = n.NameValueId,
                Category = n.Category,
                Value = n.Value,
                Label = n.Label ?? n.Value,
                SortOrder = n.SortOrder
            })
            .ToListAsync();

        return Ok(items);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> Create(CreateNameValueDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Category))
            return BadRequest("Category is required.");

        if (string.IsNullOrWhiteSpace(dto.Value))
            return BadRequest("Value is required.");

        // ── Uniqueness check: Category + Value must be unique ──
        var exists = await _context.NameValues.AnyAsync(n =>
            n.Category.ToLower() == dto.Category.ToLower().Trim() &&
            n.Value.ToLower() == dto.Value.ToLower().Trim() &&
            n.IsActive);

        if (exists)
            return BadRequest($"\"{dto.Value}\" already exists in \"{dto.Category}\".");

        var item = new NameValue
        {
            Category = dto.Category.Trim(),
            Value = dto.Value.Trim(),
            Label = dto.Label?.Trim(),
            SortOrder = dto.SortOrder,
            IsActive = true
        };

        _context.NameValues.Add(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateNameValueDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Value))
            return BadRequest("Value is required.");

        var item = await _context.NameValues.FindAsync(id);
        if (item == null) return NotFound();

        // ── Uniqueness check: exclude current record ──
        var exists = await _context.NameValues.AnyAsync(n =>
            n.Category.ToLower() == item.Category.ToLower() &&
            n.Value.ToLower() == dto.Value.ToLower().Trim() &&
            n.IsActive &&
            n.NameValueId != id);

        if (exists)
            return BadRequest($"\"{dto.Value}\" already exists in \"{item.Category}\".");

        item.Value = dto.Value.Trim();
        item.Label = dto.Label?.Trim();
        item.SortOrder = dto.SortOrder;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var item = await _context.NameValues.FindAsync(id);
        if (item == null) return NotFound();

        item.IsActive = false;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}