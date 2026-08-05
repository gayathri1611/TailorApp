using TailorApp.API.DTOs;

namespace TailorApp.API.Services;

public interface IMeasurementService
{
    Task<IEnumerable<MeasurementDto>> GetAllAsync();
    Task<IEnumerable<MeasurementDto>> GetByCustomerAsync(int customerId);
    Task<MeasurementDto?> GetByIdAsync(int id);
    Task<(string? error, MeasurementDto? result)> CreateAsync(CreateMeasurementDto dto);
    Task<(string? error, bool found)> UpdateAsync(int id, UpdateMeasurementDto dto);
    Task<bool> DeleteAsync(int id);
}
