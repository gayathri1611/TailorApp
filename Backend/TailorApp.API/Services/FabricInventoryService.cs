using TailorApp.API.DTOs;
using TailorApp.API.Models;
using TailorApp.API.Repositories.Interfaces;

namespace TailorApp.API.Services;

public class FabricInventoryService : IFabricInventoryService
{
    private readonly IFabricInventoryRepository _repo;

    public FabricInventoryService(IFabricInventoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<FabricInventoryDto>> GetAllAsync()
    {
        var fabrics = await _repo.GetAllActiveAsync();
        return fabrics.Select(MapToDto);
    }

    public async Task<FabricInventoryDto?> GetByIdAsync(int id)
    {
        var fabric = await _repo.GetActiveByIdAsync(id);
        return fabric == null ? null : MapToDto(fabric);
    }

    public async Task<FabricInventoryDto> CreateAsync(CreateFabricInventoryDto dto)
    {
        var fabric = new FabricInventory
        {
            Name = dto.Name,
            FabricType = dto.FabricType,
            Color = dto.Color,
            Supplier = dto.Supplier,
            QuantityInMeters = dto.QuantityInMeters,
            QuantityInItems = dto.QuantityInItems,
            LowStockThresholdMeters = dto.LowStockThresholdMeters,
            LowStockThresholdItems = dto.LowStockThresholdItems,
            PricePerMeter = dto.PricePerMeter,
            Notes = dto.Notes,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        await _repo.AddAsync(fabric);
        await _repo.SaveChangesAsync();

        fabric.FabricCode = $"FAB{fabric.FabricId:D5}";
        await _repo.SaveChangesAsync();

        return MapToDto(fabric);
    }

    public async Task<bool> UpdateAsync(int id, UpdateFabricInventoryDto dto)
    {
        var fabric = await _repo.FindAsync(id);
        if (fabric == null) return false;

        fabric.Name = dto.Name;
        fabric.FabricType = dto.FabricType;
        fabric.Color = dto.Color;
        fabric.Supplier = dto.Supplier;
        fabric.QuantityInMeters = dto.QuantityInMeters;
        fabric.QuantityInItems = dto.QuantityInItems;
        fabric.LowStockThresholdMeters = dto.LowStockThresholdMeters;
        fabric.LowStockThresholdItems = dto.LowStockThresholdItems;
        fabric.PricePerMeter = dto.PricePerMeter;
        fabric.Notes = dto.Notes;

        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AdjustStockAsync(int id, AdjustStockDto dto)
    {
        var fabric = await _repo.FindAsync(id);
        if (fabric == null) return false;

        fabric.QuantityInMeters += dto.MetersAdjustment;
        fabric.QuantityInItems += dto.ItemsAdjustment;

        if (fabric.QuantityInMeters < 0) fabric.QuantityInMeters = 0;
        if (fabric.QuantityInItems < 0) fabric.QuantityInItems = 0;

        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var fabric = await _repo.FindAsync(id);
        if (fabric == null) return false;

        fabric.IsActive = false;
        await _repo.SaveChangesAsync();
        return true;
    }

    private static FabricInventoryDto MapToDto(FabricInventory f) => new()
    {
        FabricId = f.FabricId,
        FabricCode = f.FabricCode,
        Name = f.Name,
        FabricType = f.FabricType,
        Color = f.Color,
        Supplier = f.Supplier,
        QuantityInMeters = f.QuantityInMeters,
        QuantityInItems = f.QuantityInItems,
        LowStockThresholdMeters = f.LowStockThresholdMeters,
        LowStockThresholdItems = f.LowStockThresholdItems,
        PricePerMeter = f.PricePerMeter,
        Notes = f.Notes,
        IsLowStockMeters = f.QuantityInMeters <= f.LowStockThresholdMeters,
        IsLowStockItems = f.QuantityInItems <= f.LowStockThresholdItems,
        CreatedDate = f.CreatedDate
    };
}
