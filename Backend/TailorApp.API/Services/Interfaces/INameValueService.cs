using TailorApp.API.DTOs;
using TailorApp.API.Models;

namespace TailorApp.API.Services;

public interface INameValueService
{
    Task<IEnumerable<NameValueDto>> GetAllAsync();
    Task<IEnumerable<NameValueDto>> GetByCategoryAsync(string category);
    Task<(string? error, NameValue? item)> CreateAsync(CreateNameValueDto dto);
    Task<(string? error, bool found)> UpdateAsync(int id, UpdateNameValueDto dto);
    Task<bool> DeleteAsync(int id);
}
