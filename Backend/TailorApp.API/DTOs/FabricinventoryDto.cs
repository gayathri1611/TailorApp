using System.ComponentModel.DataAnnotations;

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
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FabricType { get; set; }

    [MaxLength(100)]
    public string? Color { get; set; }

    [MaxLength(200)]
    public string? Supplier { get; set; }

    [Range(0, double.MaxValue)]
    public decimal QuantityInMeters { get; set; }

    [Range(0, int.MaxValue)]
    public int QuantityInItems { get; set; }

    [Range(0, double.MaxValue)]
    public decimal LowStockThresholdMeters { get; set; } = 5;

    [Range(0, int.MaxValue)]
    public int LowStockThresholdItems { get; set; } = 2;

    [Range(0, double.MaxValue)]
    public decimal PricePerMeter { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}

public class UpdateFabricInventoryDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FabricType { get; set; }

    [MaxLength(100)]
    public string? Color { get; set; }

    [MaxLength(200)]
    public string? Supplier { get; set; }

    [Range(0, double.MaxValue)]
    public decimal QuantityInMeters { get; set; }

    [Range(0, int.MaxValue)]
    public int QuantityInItems { get; set; }

    [Range(0, double.MaxValue)]
    public decimal LowStockThresholdMeters { get; set; }

    [Range(0, int.MaxValue)]
    public int LowStockThresholdItems { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PricePerMeter { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}

public class AdjustStockDto
{
    public decimal MetersAdjustment { get; set; }
    public int ItemsAdjustment { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }
}
