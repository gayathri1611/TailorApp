using TailorApp.API.Models;

namespace TailorApp.API.Repositories.Interfaces;

public interface INameValueRepository
{
    Task<IEnumerable<NameValue>> GetAllActiveAsync();
    Task<IEnumerable<NameValue>> GetActiveByCategoryAsync(string category);
    Task<bool> ExistsActiveAsync(string category, string value, int? excludeId = null);
    Task<NameValue?> FindAsync(int id);
    Task AddAsync(NameValue item);
    Task SaveChangesAsync();
}
