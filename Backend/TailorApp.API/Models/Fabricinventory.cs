using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TailorApp.API.Models;

public class FabricInventory
{
    [Key]
    public int FabricId { get; set; }
    public string FabricCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? FabricType { get; set; }
    public string? Color { get; set; }
    public string? Supplier { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal QuantityInMeters { get; set; } = 0;

    public int QuantityInItems { get; set; } = 0;

    [Column(TypeName = "decimal(10,2)")]
    public decimal LowStockThresholdMeters { get; set; } = 5;

    public int LowStockThresholdItems { get; set; } = 2;

    [Column(TypeName = "decimal(10,2)")]
    public decimal PricePerMeter { get; set; } = 0;

    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}