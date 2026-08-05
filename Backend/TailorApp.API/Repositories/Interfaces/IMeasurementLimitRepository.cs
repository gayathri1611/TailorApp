using TailorApp.API.Models;

namespace TailorApp.API.Repositories.Interfaces;

public interface IMeasurementLimitRepository
{
    Task<IEnumerable<NameValue>> GetAllLimitsAsync();
    Task<NameValue?> GetLimitByFieldAsync(string fieldName);
    Task AddAsync(NameValue item);
    Task SaveChangesAsync();
}
