using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TailorApp.API.Data;
using TailorApp.API.DTOs;
using TailorApp.API.Models;

namespace TailorApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FabricInventoryController : ControllerBase
{
    private readonly TailorAppDbContext _context;

    public FabricInventoryController(TailorAppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FabricInventoryDto>>> GetAll()
    {
        var fabrics = await _context.FabricInventories
            .Where(f => f.IsActive)
            .Select(f => new FabricInventoryDto
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
            })
            .ToListAsync();

        return Ok(fabrics);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FabricInventoryDto>> GetById(int id)
    {
        var f = await _context.FabricInventories
            .Where(f => f.FabricId == id && f.IsActive)
            .FirstOrDefaultAsync();

        if (f == null) return NotFound();

        return Ok(new FabricInventoryDto
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
        });
    }

    [HttpPost]
    public async Task<ActionResult<FabricInventoryDto>> Create(CreateFabricInventoryDto dto)
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

        _context.FabricInventories.Add(fabric);
        await _context.SaveChangesAsync();

        fabric.FabricCode = $"FAB{fabric.FabricId:D5}";
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = fabric.FabricId }, new FabricInventoryDto
        {
            FabricId = fabric.FabricId,
            FabricCode = fabric.FabricCode,
            Name = fabric.Name,
            FabricType = fabric.FabricType,
            Color = fabric.Color,
            Supplier = fabric.Supplier,
            QuantityInMeters = fabric.QuantityInMeters,
            QuantityInItems = fabric.QuantityInItems,
            LowStockThresholdMeters = fabric.LowStockThresholdMeters,
            LowStockThresholdItems = fabric.LowStockThresholdItems,
            PricePerMeter = fabric.PricePerMeter,
            Notes = fabric.Notes,
            IsLowStockMeters = fabric.QuantityInMeters <= fabric.LowStockThresholdMeters,
            IsLowStockItems = fabric.QuantityInItems <= fabric.LowStockThresholdItems,
            CreatedDate = fabric.CreatedDate
        });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateFabricInventoryDto dto)
    {
        var fabric = await _context.FabricInventories.FindAsync(id);
        if (fabric == null) return NotFound();

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

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}/adjust-stock")]
    public async Task<ActionResult> AdjustStock(int id, AdjustStockDto dto)
    {
        var fabric = await _context.FabricInventories.FindAsync(id);
        if (fabric == null) return NotFound();

        fabric.QuantityInMeters += dto.MetersAdjustment;
        fabric.QuantityInItems += dto.ItemsAdjustment;

        if (fabric.QuantityInMeters < 0) fabric.QuantityInMeters = 0;
        if (fabric.QuantityInItems < 0) fabric.QuantityInItems = 0;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var fabric = await _context.FabricInventories.FindAsync(id);
        if (fabric == null) return NotFound();

        fabric.IsActive = false;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
