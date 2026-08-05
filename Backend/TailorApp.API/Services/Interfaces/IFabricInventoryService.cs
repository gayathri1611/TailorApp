using TailorApp.API.DTOs;

namespace TailorApp.API.Services;

public interface IFabricInventoryService
{
    Task<IEnumerable<FabricInventoryDto>> GetAllAsync();
    Task<FabricInventoryDto?> GetByIdAsync(int id);
    Task<FabricInventoryDto> CreateAsync(CreateFabricInventoryDto dto);
    Task<bool> UpdateAsync(int id, UpdateFabricInventoryDto dto);
    Task<bool> AdjustStockAsync(int id, AdjustStockDto dto);
    Task<bool> DeleteAsync(int id);
}
