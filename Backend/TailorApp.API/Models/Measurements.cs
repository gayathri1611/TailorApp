using System.ComponentModel.DataAnnotations.Schema;

namespace TailorApp.API.Models;

public class Measurement
{
    public int MeasurementId { get; set; }
    public string MeasurementCode { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int ShopId { get; set; }

    // Upper body
    [Column(TypeName = "decimal(5,2)")]
    public decimal? Chest { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? Shoulder { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? SleeveLength { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? ArmHole { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? Neck { get; set; }

    // Lower body
    [Column(TypeName = "decimal(5,2)")]
    public decimal? Waist { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? Hip { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? Thigh { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? Knee { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? InseamLength { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? OutseamLength { get; set; }

    // Full body
    [Column(TypeName = "decimal(5,2)")]
    public decimal? Height { get; set; }

    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Customer? Customer { get; set; }
}