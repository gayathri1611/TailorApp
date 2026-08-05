using TailorApp.API.Models;

namespace TailorApp.API.Repositories.Interfaces;

public interface IFabricInventoryRepository
{
    Task<IEnumerable<FabricInventory>> GetAllActiveAsync();
    Task<FabricInventory?> GetActiveByIdAsync(int id);
    Task<FabricInventory?> FindAsync(int id);
    Task AddAsync(FabricInventory fabric);
    Task SaveChangesAsync();
}
