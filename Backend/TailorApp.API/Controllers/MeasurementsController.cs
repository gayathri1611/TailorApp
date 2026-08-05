using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorApp.API.DTOs;
using TailorApp.API.Services;

namespace TailorApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MeasurementsController : ControllerBase
{
    private readonly IMeasurementService _measurementService;

    public MeasurementsController(IMeasurementService measurementService)
    {
        _measurementService = measurementService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MeasurementDto>>> GetMeasurements()
    {
        return Ok(await _measurementService.GetAllAsync());
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<MeasurementDto>>> GetByCustomer(int customerId)
    {
        return Ok(await _measurementService.GetByCustomerAsync(customerId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MeasurementDto>> GetMeasurement(int id)
    {
        var m = await _measurementService.GetByIdAsync(id);
        if (m == null) return NotFound();
        return Ok(m);
    }

    [HttpPost]
    public async Task<ActionResult<MeasurementDto>> CreateMeasurement(CreateMeasurementDto dto)
    {
        var (error, result) = await _measurementService.CreateAsync(dto);
        if (error != null) return BadRequest(error);
        return CreatedAtAction(nameof(GetMeasurement), new { id = result!.MeasurementId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMeasurement(int id, UpdateMeasurementDto dto)
    {
        var (error, found) = await _measurementService.UpdateAsync(id, dto);
        if (!found) return NotFound();
        if (error != null) return BadRequest(error);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMeasurement(int id)
    {
        if (!await _measurementService.DeleteAsync(id)) return NotFound();
        return NoContent();
    }
}
