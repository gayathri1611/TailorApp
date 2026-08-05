using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorApp.API.DTOs;
using TailorApp.API.Services;

namespace TailorApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MeasurementLimitsController : ControllerBase
{
    private readonly IMeasurementLimitService _limitService;

    public MeasurementLimitsController(IMeasurementLimitService limitService)
    {
        _limitService = limitService;
    }

    [HttpGet]
    public async Task<ActionResult<Dictionary<string, object>>> GetLimits()
    {
        return Ok(await _limitService.GetLimitsAsync());
    }

    [HttpPut("{fieldName}")]
    public async Task<ActionResult> SetLimit(string fieldName, [FromBody] SetLimitDto dto)
    {
        var error = await _limitService.SetLimitAsync(fieldName, dto);
        if (error != null) return BadRequest(error);
        return Ok();
    }
}
