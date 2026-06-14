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
    public async Task<ActionResult<MeasurementDto>> CreateMeasurement(CreateMeasurementDto dto)
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

        await _context.Entry(measurement).Reference(m => m.Customer).LoadAsync();

        return CreatedAtAction(nameof(GetMeasurement), new { id = measurement.MeasurementId }, new MeasurementDto
        {
            MeasurementId = measurement.MeasurementId,
            MeasurementCode = measurement.MeasurementCode,
            CustomerId = measurement.CustomerId,
            ShopId = measurement.ShopId,
            CustomerName = measurement.Customer != null
                ? measurement.Customer.FirstName + " " + (measurement.Customer.LastName ?? "")
                : "",
            Chest = measurement.Chest,
            Shoulder = measurement.Shoulder,
            SleeveLength = measurement.SleeveLength,
            ArmHole = measurement.ArmHole,
            Neck = measurement.Neck,
            Waist = measurement.Waist,
            Hip = measurement.Hip,
            Thigh = measurement.Thigh,
            Knee = measurement.Knee,
            InseamLength = measurement.InseamLength,
            OutseamLength = measurement.OutseamLength,
            Height = measurement.Height,
            Notes = measurement.Notes,
            CreatedDate = measurement.CreatedDate
        });
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
    // Add this private helper method to MeasurementsController:

    private async Task<string?> ValidateMeasurements(
        decimal? chest, decimal? shoulder, decimal? sleeveLength,
        decimal? armHole, decimal? neck, decimal? waist,
        decimal? hip, decimal? thigh, decimal? knee,
        decimal? inseamLength, decimal? outseamLength, decimal? height)
    {
        var limits = await _context.NameValues
            .Where(n => n.Category == "MeasurementLimit" && n.IsActive)
            .ToListAsync();

        var limitMap = new Dictionary<string, (decimal min, decimal max)>();
        foreach (var l in limits)
        {
            var parts = l.Value.Split(':');
            if (parts.Length == 3 &&
                decimal.TryParse(parts[1], out var min) &&
                decimal.TryParse(parts[2], out var max))
                limitMap[parts[0].ToLower()] = (min, max);
        }

        // Default limits if not configured
        decimal defaultMin = 0, defaultMax = 300;

        var fields = new Dictionary<string, decimal?>
        {
            ["chest"] = chest,
            ["shoulder"] = shoulder,
            ["sleevelength"] = sleeveLength,
            ["armhole"] = armHole,
            ["neck"] = neck,
            ["waist"] = waist,
            ["hip"] = hip,
            ["thigh"] = thigh,
            ["knee"] = knee,
            ["inseamlength"] = inseamLength,
            ["outseamlength"] = outseamLength,
            ["height"] = height
        };

        var displayNames = new Dictionary<string, string>
        {
            ["chest"] = "Chest",
            ["shoulder"] = "Shoulder",
            ["sleevelength"] = "Sleeve Length",
            ["armhole"] = "Arm Hole",
            ["neck"] = "Neck",
            ["waist"] = "Waist",
            ["hip"] = "Hip",
            ["thigh"] = "Thigh",
            ["knee"] = "Knee",
            ["inseamlength"] = "Inseam",
            ["outseamlength"] = "Outseam",
            ["height"] = "Height"
        };

        foreach (var (key, value) in fields)
        {
            if (value == null) continue;

            var (min, max) = limitMap.ContainsKey(key)
                ? limitMap[key]
                : (defaultMin, defaultMax);

            var name = displayNames[key];

            if (value < min)
                return $"{name} cannot be less than {min}.";
            if (value > max)
                return $"{name} cannot exceed {max}.";
        }

        return null; // valid
    }

    // ── Then in CreateMeasurement and UpdateMeasurement, call it like this: ──

    // var validationError = await ValidateMeasurements(
    //     dto.Chest, dto.Shoulder, dto.SleeveLength, dto.ArmHole, dto.Neck, dto.Waist,
    //     dto.Hip, dto.Thigh, dto.Knee, dto.InseamLength, dto.OutseamLength, dto.Height);
    // if (validationError != null) return BadRequest(validationError);
}
