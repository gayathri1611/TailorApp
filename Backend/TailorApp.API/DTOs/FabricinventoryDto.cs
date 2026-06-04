namespace TailorApp.API.DTOs;

public class FabricInventoryDto
{
    public int FabricId { get; set; }
    public string FabricCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? FabricType { get; set; }
    public string? Color { get; set; }
    public string? Supplier { get; set; }
    public decimal QuantityInMeters { get; set; }
    public int QuantityInItems { get; set; }
    public decimal LowStockThresholdMeters { get; set; }
    public int LowStockThresholdItems { get; set; }
    public decimal PricePerMeter { get; set; }
    public string? Notes { get; set; }
    public bool IsLowStockMeters { get; set; }
    public bool IsLowStockItems { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class CreateFabricInventoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? FabricType { get; set; }
    public string? Color { get; set; }
    public string? Supplier { get; set; }
    public decimal QuantityInMeters { get; set; }
    public int QuantityInItems { get; set; }
    public decimal LowStockThresholdMeters { get; set; } = 5;
    public int LowStockThresholdItems { get; set; } = 2;
    public decimal PricePerMeter { get; set; }
    public string? Notes { get; set; }
}

public class UpdateFabricInventoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? FabricType { get; set; }
    public string? Color { get; set; }
    public string? Supplier { get; set; }
    public decimal QuantityInMeters { get; set; }
    public int QuantityInItems { get; set; }
    public decimal LowStockThresholdMeters { get; set; }
    public int LowStockThresholdItems { get; set; }
    public decimal PricePerMeter { get; set; }
    public string? Notes { get; set; }
}

public class AdjustStockDto
{
    public decimal MetersAdjustment { get; set; }
    public int ItemsAdjustment { get; set; }
    public string? Reason { get; set; }
}