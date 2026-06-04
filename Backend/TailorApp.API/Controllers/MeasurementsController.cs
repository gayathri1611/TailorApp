using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TailorApp.API.Data;
using TailorApp.API.DTOs;
using TailorApp.API.Models;

namespace TailorApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeasurementsController : ControllerBase
{
    private readonly TailorAppDbContext _context;

    public MeasurementsController(TailorAppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MeasurementDto>>> GetMeasurements()
    {
        var measurements = await _context.Measurements
            .Include(m => m.Customer)
            .Where(m => m.IsActive)
            .Select(m => new MeasurementDto
            {
                MeasurementId = m.MeasurementId,
                MeasurementCode = m.MeasurementCode,
                CustomerId = m.CustomerId,
                ShopId = m.ShopId,
                CustomerName = m.Customer!.FirstName + " " + (m.Customer.LastName ?? ""),
                Chest = m.Chest,
                Shoulder = m.Shoulder,
                SleeveLength = m.SleeveLength,
                ArmHole = m.ArmHole,
                Neck = m.Neck,
                Waist = m.Waist,
                Hip = m.Hip,
                Thigh = m.Thigh,
                Knee = m.Knee,
                InseamLength = m.InseamLength,
                OutseamLength = m.OutseamLength,
                Height = m.Height,
                Notes = m.Notes,
                CreatedDate = m.CreatedDate
            })
            .ToListAsync();

        return Ok(measurements);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<MeasurementDto>>> GetByCustomer(int customerId)
    {
        var measurements = await _context.Measurements
            .Include(m => m.Customer)
            .Where(m => m.CustomerId == customerId && m.IsActive)
            .Select(m => new MeasurementDto
            {
                MeasurementId = m.MeasurementId,
                MeasurementCode = m.MeasurementCode,
                CustomerId = m.CustomerId,
                ShopId = m.ShopId,
                CustomerName = m.Customer!.FirstName + " " + (m.Customer.LastName ?? ""),
                Chest = m.Chest,
                Shoulder = m.Shoulder,
                SleeveLength = m.SleeveLength,
                ArmHole = m.ArmHole,
                Neck = m.Neck,
                Waist = m.Waist,
                Hip = m.Hip,
                Thigh = m.Thigh,
                Knee = m.Knee,
                InseamLength = m.InseamLength,
                OutseamLength = m.OutseamLength,
                Height = m.Height,
                Notes = m.Notes,
                CreatedDate = m.CreatedDate
            })
            .ToListAsync();

        return Ok(measurements);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MeasurementDto>> GetMeasurement(int id)
    {
        var m = await _context.Measurements
            .Include(m => m.Customer)
            .Where(m => m.MeasurementId == id && m.IsActive)
            .FirstOrDefaultAsync();

        if (m == null) return NotFound();

        return Ok(new MeasurementDto
        {
            MeasurementId = m.MeasurementId,
            MeasurementCode = m.MeasurementCode,
            CustomerId = m.CustomerId,
            ShopId = m.ShopId,
            CustomerName = m.Customer!.FirstName + " " + (m.Customer.LastName ?? ""),
            Chest = m.Chest,
            Shoulder = m.Shoulder,
            SleeveLength = m.SleeveLength,
            ArmHole = m.ArmHole,
            Neck = m.Neck,
            Waist = m.Waist,
            Hip = m.Hip,
            Thigh = m.Thigh,
            Knee = m.Knee,
            InseamLength = m.InseamLength,
            OutseamLength = m.OutseamLength,
            Height = m.Height,
            Notes = m.Notes,
            CreatedDate = m.CreatedDate
        });
    }

    [HttpPost]
    public async Task<ActionResult> CreateMeasurement(CreateMeasurementDto dto)
    {
        var measurement = new Measurement
        {
            CustomerId = dto.CustomerId,
            ShopId = dto.ShopId,
            Chest = dto.Chest,
            Shoulder = dto.Shoulder,
            SleeveLength = dto.SleeveLength,
            ArmHole = dto.ArmHole,
            Neck = dto.Neck,
            Waist = dto.Waist,
            Hip = dto.Hip,
            Thigh = dto.Thigh,
            Knee = dto.Knee,
            InseamLength = dto.InseamLength,
            OutseamLength = dto.OutseamLength,
            Height = dto.Height,
            Notes = dto.Notes,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        _context.Measurements.Add(measurement);
        await _context.SaveChangesAsync();

        measurement.MeasurementCode = $"MEAS{measurement.MeasurementId:D5}";
        await _context.SaveChangesAsync();

        return Ok(measurement);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMeasurement(int id, UpdateMeasurementDto dto)
    {
        var measurement = await _context.Measurements.FindAsync(id);
        if (measurement == null) return NotFound();

        measurement.Chest = dto.Chest;
        measurement.Shoulder = dto.Shoulder;
        measurement.SleeveLength = dto.SleeveLength;
        measurement.ArmHole = dto.ArmHole;
        measurement.Neck = dto.Neck;
        measurement.Waist = dto.Waist;
        measurement.Hip = dto.Hip;
        measurement.Thigh = dto.Thigh;
        measurement.Knee = dto.Knee;
        measurement.InseamLength = dto.InseamLength;
        measurement.OutseamLength = dto.OutseamLength;
        measurement.Height = dto.Height;
        measurement.Notes = dto.Notes;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMeasurement(int id)
    {
        var measurement = await _context.Measurements.FindAsync(id);
        if (measurement == null) return NotFound();

        measurement.IsActive = false;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}