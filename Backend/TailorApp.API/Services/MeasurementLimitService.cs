using TailorApp.API.DTOs;
using TailorApp.API.Models;
using TailorApp.API.Repositories.Interfaces;

namespace TailorApp.API.Services;

public class MeasurementLimitService : IMeasurementLimitService
{
    private readonly IMeasurementLimitRepository _repo;

    public MeasurementLimitService(IMeasurementLimitRepository repo)
    {
        _repo = repo;
    }

    public async Task<Dictionary<string, object>> GetLimitsAsync()
    {
        var limits = await _repo.GetAllLimitsAsync();

        var result = new Dictionary<string, object>();
        foreach (var l in limits)
        {
            var parts = l.Value.Split(':');
            if (parts.Length == 3 &&
                decimal.TryParse(parts[1], out var min) &&
                decimal.TryParse(parts[2], out var max))
            {
                result[parts[0]] = new { min, max, nameValueId = l.NameValueId };
            }
        }

        return result;
    }

    public async Task<string?> SetLimitAsync(string fieldName, SetLimitDto dto)
    {
        if (dto.Min < 0) return "Minimum cannot be negative.";
        if (dto.Max <= dto.Min) return "Maximum must be greater than minimum.";

        var existing = await _repo.GetLimitByFieldAsync(fieldName);
        var encoded = $"{fieldName}:{dto.Min}:{dto.Max}";

        if (existing != null)
        {
            existing.Value = encoded;
            existing.Label = $"{fieldName} ({dto.Min}–{dto.Max})";
        }
        else
        {
            await _repo.AddAsync(new NameValue
            {
                Category = "MeasurementLimit",
                Value = encoded,
                Label = $"{fieldName} ({dto.Min}–{dto.Max})",
                SortOrder = 0,
                IsActive = true
            });
        }

        await _repo.SaveChangesAsync();
        return null;
    }
}
