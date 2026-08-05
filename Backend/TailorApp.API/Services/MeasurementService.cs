using TailorApp.API.DTOs;
using TailorApp.API.Models;
using TailorApp.API.Repositories.Interfaces;

namespace TailorApp.API.Services;

public class MeasurementService : IMeasurementService
{
    private readonly IMeasurementRepository _repo;
    private readonly IMeasurementLimitRepository _limitRepo;

    public MeasurementService(IMeasurementRepository repo, IMeasurementLimitRepository limitRepo)
    {
        _repo = repo;
        _limitRepo = limitRepo;
    }

    public async Task<IEnumerable<MeasurementDto>> GetAllAsync()
    {
        var measurements = await _repo.GetAllActiveAsync();
        return measurements.Select(MapToDto);
    }

    public async Task<IEnumerable<MeasurementDto>> GetByCustomerAsync(int customerId)
    {
        var measurements = await _repo.GetActiveByCustomerAsync(customerId);
        return measurements.Select(MapToDto);
    }

    public async Task<MeasurementDto?> GetByIdAsync(int id)
    {
        var m = await _repo.GetActiveByIdAsync(id);
        return m == null ? null : MapToDto(m);
    }

    public async Task<(string? error, MeasurementDto? result)> CreateAsync(CreateMeasurementDto dto)
    {
        var validationError = await ValidateMeasurementsAsync(
            dto.Chest, dto.Shoulder, dto.SleeveLength, dto.ArmHole, dto.Neck,
            dto.Waist, dto.Hip, dto.Thigh, dto.Knee, dto.InseamLength, dto.OutseamLength, dto.Height);

        if (validationError != null) return (validationError, null);

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

        await _repo.AddAsync(measurement);
        await _repo.SaveChangesAsync();

        measurement.MeasurementCode = $"MEAS{measurement.MeasurementId:D5}";
        await _repo.SaveChangesAsync();

        await _repo.LoadCustomerAsync(measurement);

        return (null, MapToDto(measurement));
    }

    public async Task<(string? error, bool found)> UpdateAsync(int id, UpdateMeasurementDto dto)
    {
        var measurement = await _repo.FindAsync(id);
        if (measurement == null) return (null, false);

        var validationError = await ValidateMeasurementsAsync(
            dto.Chest, dto.Shoulder, dto.SleeveLength, dto.ArmHole, dto.Neck,
            dto.Waist, dto.Hip, dto.Thigh, dto.Knee, dto.InseamLength, dto.OutseamLength, dto.Height);

        if (validationError != null) return (validationError, true);

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

        await _repo.SaveChangesAsync();
        return (null, true);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var measurement = await _repo.FindAsync(id);
        if (measurement == null) return false;

        measurement.IsActive = false;
        await _repo.SaveChangesAsync();
        return true;
    }

    private async Task<string?> ValidateMeasurementsAsync(
        decimal? chest, decimal? shoulder, decimal? sleeveLength,
        decimal? armHole, decimal? neck, decimal? waist,
        decimal? hip, decimal? thigh, decimal? knee,
        decimal? inseamLength, decimal? outseamLength, decimal? height)
    {
        var limits = await _limitRepo.GetAllLimitsAsync();

        var limitMap = new Dictionary<string, (decimal min, decimal max)>();
        foreach (var l in limits)
        {
            var parts = l.Value.Split(':');
            if (parts.Length == 3 &&
                decimal.TryParse(parts[1], out var min) &&
                decimal.TryParse(parts[2], out var max))
                limitMap[parts[0].ToLower()] = (min, max);
        }

        decimal defaultMin = 0, defaultMax = 300;

        var fields = new Dictionary<string, decimal?>
        {
            ["chest"] = chest, ["shoulder"] = shoulder, ["sleevelength"] = sleeveLength,
            ["armhole"] = armHole, ["neck"] = neck, ["waist"] = waist,
            ["hip"] = hip, ["thigh"] = thigh, ["knee"] = knee,
            ["inseamlength"] = inseamLength, ["outseamlength"] = outseamLength, ["height"] = height
        };

        var displayNames = new Dictionary<string, string>
        {
            ["chest"] = "Chest", ["shoulder"] = "Shoulder", ["sleevelength"] = "Sleeve Length",
            ["armhole"] = "Arm Hole", ["neck"] = "Neck", ["waist"] = "Waist",
            ["hip"] = "Hip", ["thigh"] = "Thigh", ["knee"] = "Knee",
            ["inseamlength"] = "Inseam", ["outseamlength"] = "Outseam", ["height"] = "Height"
        };

        foreach (var (key, value) in fields)
        {
            if (value == null) continue;

            var (min, max) = limitMap.TryGetValue(key, out var lim) ? lim : (defaultMin, defaultMax);
            var name = displayNames[key];

            if (value < min) return $"{name} cannot be less than {min}.";
            if (value > max) return $"{name} cannot exceed {max}.";
        }

        return null;
    }

    private static MeasurementDto MapToDto(Measurement m) => new()
    {
        MeasurementId = m.MeasurementId,
        MeasurementCode = m.MeasurementCode,
        CustomerId = m.CustomerId,
        ShopId = m.ShopId,
        CustomerName = m.Customer != null ? m.Customer.FirstName + " " + (m.Customer.LastName ?? "") : "",
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
    };
}
