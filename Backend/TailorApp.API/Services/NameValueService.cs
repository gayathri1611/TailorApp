using TailorApp.API.DTOs;
using TailorApp.API.Models;
using TailorApp.API.Repositories.Interfaces;

namespace TailorApp.API.Services;

public class NameValueService : INameValueService
{
    private readonly INameValueRepository _repo;

    public NameValueService(INameValueRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<NameValueDto>> GetAllAsync()
    {
        var items = await _repo.GetAllActiveAsync();
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<NameValueDto>> GetByCategoryAsync(string category)
    {
        var items = await _repo.GetActiveByCategoryAsync(category);
        return items.Select(MapToDto);
    }

    public async Task<(string? error, NameValue? item)> CreateAsync(CreateNameValueDto dto)
    {
        var exists = await _repo.ExistsActiveAsync(dto.Category.Trim(), dto.Value.Trim());
        if (exists)
            return ($"\"{dto.Value}\" already exists in \"{dto.Category}\".", null);

        var item = new NameValue
        {
            Category = dto.Category.Trim(),
            Value = dto.Value.Trim(),
            Label = dto.Label?.Trim(),
            SortOrder = dto.SortOrder,
            IsActive = true
        };

        await _repo.AddAsync(item);
        await _repo.SaveChangesAsync();
        return (null, item);
    }

    public async Task<(string? error, bool found)> UpdateAsync(int id, UpdateNameValueDto dto)
    {
        var item = await _repo.FindAsync(id);
        if (item == null) return (null, false);

        var exists = await _repo.ExistsActiveAsync(item.Category, dto.Value.Trim(), excludeId: id);
        if (exists)
            return ($"\"{dto.Value}\" already exists in \"{item.Category}\".", true);

        item.Value = dto.Value.Trim();
        item.Label = dto.Label?.Trim();
        item.SortOrder = dto.SortOrder;

        await _repo.SaveChangesAsync();
        return (null, true);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _repo.FindAsync(id);
        if (item == null) return false;

        item.IsActive = false;
        await _repo.SaveChangesAsync();
        return true;
    }

    private static NameValueDto MapToDto(NameValue n) => new()
    {
        NameValueId = n.NameValueId,
        Category = n.Category,
        Value = n.Value,
        Label = n.Label ?? n.Value,
        SortOrder = n.SortOrder
    };
}
