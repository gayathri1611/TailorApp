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
public class MeasurementLimitsController : ControllerBase
{
    private readonly TailorAppDbContext _context;

    public MeasurementLimitsController(TailorAppDbContext context)
    {
        _context = context;
    }

    // GET all limits as key-value pairs
    [HttpGet]
    public async Task<ActionResult<Dictionary<string, object>>> GetLimits()
    {
        var limits = await _context.NameValues
            .Where(n => n.Category == "MeasurementLimit" && n.IsActive)
            .ToListAsync();

        var result = new Dictionary<string, object>();
        foreach (var l in limits)
        {
            // Value format: "fieldName:min:max"  e.g. "chest:0:200"
            var parts = l.Value.Split(':');
            if (parts.Length == 3 &&
                decimal.TryParse(parts[1], out var min) &&
                decimal.TryParse(parts[2], out var max))
            {
                result[parts[0]] = new { min, max, nameValueId = l.NameValueId };
            }
        }

        return Ok(result);
    }

    // PUT — upsert a limit for a field
    [HttpPut("{fieldName}")]
    public async Task<ActionResult> SetLimit(string fieldName, [FromBody] SetLimitDto dto)
    {
        if (dto.Min < 0)
            return BadRequest("Minimum cannot be negative.");
        if (dto.Max <= dto.Min)
            return BadRequest("Maximum must be greater than minimum.");

        var existing = await _context.NameValues
            .Where(n => n.Category == "MeasurementLimit" &&
                        n.Value.StartsWith(fieldName + ":") &&
                        n.IsActive)
            .FirstOrDefaultAsync();

        var encoded = $"{fieldName}:{dto.Min}:{dto.Max}";

        if (existing != null)
        {
            existing.Value = encoded;
            existing.Label = $"{fieldName} ({dto.Min}–{dto.Max})";
        }
        else
        {
            _context.NameValues.Add(new NameValue
            {
                Category = "MeasurementLimit",
                Value = encoded,
                Label = $"{fieldName} ({dto.Min}–{dto.Max})",
                SortOrder = 0,
                IsActive = true
            });
        }

        await _context.SaveChangesAsync();
        return Ok();
    }
}

public class SetLimitDto
{
    public decimal Min { get; set; } = 0;
    public decimal Max { get; set; } = 300;
}