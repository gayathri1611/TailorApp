using TailorApp.API.DTOs;

namespace TailorApp.API.Services;

public interface IMeasurementLimitService
{
    Task<Dictionary<string, object>> GetLimitsAsync();
    Task<string?> SetLimitAsync(string fieldName, SetLimitDto dto);
}
