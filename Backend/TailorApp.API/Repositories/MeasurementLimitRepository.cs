using Microsoft.EntityFrameworkCore;
using TailorApp.API.Data;
using TailorApp.API.Models;
using TailorApp.API.Repositories.Interfaces;

namespace TailorApp.API.Repositories;

public class MeasurementLimitRepository : IMeasurementLimitRepository
{
    private readonly TailorAppDbContext _context;

    public MeasurementLimitRepository(TailorAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NameValue>> GetAllLimitsAsync() =>
        await _context.NameValues
            .Where(n => n.Category == "MeasurementLimit" && n.IsActive)
            .ToListAsync();

    public async Task<NameValue?> GetLimitByFieldAsync(string fieldName) =>
        await _context.NameValues
            .FirstOrDefaultAsync(n =>
                n.Category == "MeasurementLimit" &&
                n.Value.StartsWith(fieldName + ":") &&
                n.IsActive);

    public Task AddAsync(NameValue item)
    {
        _context.NameValues.Add(item);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
