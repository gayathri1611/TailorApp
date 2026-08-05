using Microsoft.EntityFrameworkCore;
using TailorApp.API.Data;
using TailorApp.API.Models;
using TailorApp.API.Repositories.Interfaces;

namespace TailorApp.API.Repositories;

public class FabricInventoryRepository : IFabricInventoryRepository
{
    private readonly TailorAppDbContext _context;

    public FabricInventoryRepository(TailorAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FabricInventory>> GetAllActiveAsync() =>
        await _context.FabricInventories.Where(f => f.IsActive).ToListAsync();

    public async Task<FabricInventory?> GetActiveByIdAsync(int id) =>
        await _context.FabricInventories.FirstOrDefaultAsync(f => f.FabricId == id && f.IsActive);

    public async Task<FabricInventory?> FindAsync(int id) =>
        await _context.FabricInventories.FindAsync(id);

    public Task AddAsync(FabricInventory fabric)
    {
        _context.FabricInventories.Add(fabric);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
