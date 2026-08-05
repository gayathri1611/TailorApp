using Microsoft.EntityFrameworkCore;
using TailorApp.API.Data;
using TailorApp.API.Models;
using TailorApp.API.Repositories.Interfaces;

namespace TailorApp.API.Repositories;

public class MeasurementRepository : IMeasurementRepository
{
    private readonly TailorAppDbContext _context;

    public MeasurementRepository(TailorAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Measurement>> GetAllActiveAsync() =>
        await _context.Measurements
            .Include(m => m.Customer)
            .Where(m => m.IsActive)
            .ToListAsync();

    public async Task<IEnumerable<Measurement>> GetActiveByCustomerAsync(int customerId) =>
        await _context.Measurements
            .Include(m => m.Customer)
            .Where(m => m.CustomerId == customerId && m.IsActive)
            .ToListAsync();

    public async Task<Measurement?> GetActiveByIdAsync(int id) =>
        await _context.Measurements
            .Include(m => m.Customer)
            .FirstOrDefaultAsync(m => m.MeasurementId == id && m.IsActive);

    public async Task<Measurement?> FindAsync(int id) =>
        await _context.Measurements.FindAsync(id);

    public Task AddAsync(Measurement measurement)
    {
        _context.Measurements.Add(measurement);
        return Task.CompletedTask;
    }

    public async Task LoadCustomerAsync(Measurement measurement) =>
        await _context.Entry(measurement).Reference(m => m.Customer).LoadAsync();

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
