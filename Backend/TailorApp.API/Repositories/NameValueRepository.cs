using Microsoft.EntityFrameworkCore;
using TailorApp.API.Data;
using TailorApp.API.Models;
using TailorApp.API.Repositories.Interfaces;

namespace TailorApp.API.Repositories;

public class NameValueRepository : INameValueRepository
{
    private readonly TailorAppDbContext _context;

    public NameValueRepository(TailorAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NameValue>> GetAllActiveAsync() =>
        await _context.NameValues
            .Where(n => n.IsActive)
            .OrderBy(n => n.Category)
            .ThenBy(n => n.SortOrder)
            .ThenBy(n => n.Value)
            .ToListAsync();

    public async Task<IEnumerable<NameValue>> GetActiveByCategoryAsync(string category) =>
        await _context.NameValues
            .Where(n => n.Category == category && n.IsActive)
            .OrderBy(n => n.SortOrder)
            .ThenBy(n => n.Value)
            .ToListAsync();

    public async Task<bool> ExistsActiveAsync(string category, string value, int? excludeId = null)
    {
        var query = _context.NameValues.Where(n =>
            n.Category.ToLower() == category.ToLower() &&
            n.Value.ToLower() == value.ToLower() &&
            n.IsActive);

        if (excludeId.HasValue)
            query = query.Where(n => n.NameValueId != excludeId.Value);

        return await query.AnyAsync();
    }

    public async Task<NameValue?> FindAsync(int id) =>
        await _context.NameValues.FindAsync(id);

    public Task AddAsync(NameValue item)
    {
        _context.NameValues.Add(item);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
