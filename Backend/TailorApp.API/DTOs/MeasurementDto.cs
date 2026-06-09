using System.ComponentModel.DataAnnotations;

namespace TailorApp.API.DTOs;

public class MeasurementDto
{
    public int MeasurementId { get; set; }
    public string MeasurementCode { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int ShopId { get; set; }

    public decimal? Chest { get; set; }
    public decimal? Shoulder { get; set; }
    public decimal? SleeveLength { get; set; }
    public decimal? ArmHole { get; set; }
    public decimal? Neck { get; set; }

    public decimal? Waist { get; set; }
    public decimal? Hip { get; set; }
    public decimal? Thigh { get; set; }
    public decimal? Knee { get; set; }
    public decimal? InseamLength { get; set; }
    public decimal? OutseamLength { get; set; }

    public decimal? Height { get; set; }

    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class CreateMeasurementDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A valid customer is required.")]
    public int CustomerId { get; set; }

    [Range(1, int.MaxValue)]
    public int ShopId { get; set; }

    [Range(0, 999.99)]
    public decimal? Chest { get; set; }

    [Range(0, 999.99)]
    public decimal? Shoulder { get; set; }

    [Range(0, 999.99)]
    public decimal? SleeveLength { get; set; }

    [Range(0, 999.99)]
    public decimal? ArmHole { get; set; }

    [Range(0, 999.99)]
    public decimal? Neck { get; set; }

    [Range(0, 999.99)]
    public decimal? Waist { get; set; }

    [Range(0, 999.99)]
    public decimal? Hip { get; set; }

    [Range(0, 999.99)]
    public decimal? Thigh { get; set; }

    [Range(0, 999.99)]
    public decimal? Knee { get; set; }

    [Range(0, 999.99)]
    public decimal? InseamLength { get; set; }

    [Range(0, 999.99)]
    public decimal? OutseamLength { get; set; }

    [Range(0, 999.99)]
    public decimal? Height { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}

public class UpdateMeasurementDto
{
    [Range(0, 999.99)]
    public decimal? Chest { get; set; }

    [Range(0, 999.99)]
    public decimal? Shoulder { get; set; }

    [Range(0, 999.99)]
    public decimal? SleeveLength { get; set; }

    [Range(0, 999.99)]
    public decimal? ArmHole { get; set; }

    [Range(0, 999.99)]
    public decimal? Neck { get; set; }

    [Range(0, 999.99)]
    public decimal? Waist { get; set; }

    [Range(0, 999.99)]
    public decimal? Hip { get; set; }

    [Range(0, 999.99)]
    public decimal? Thigh { get; set; }

    [Range(0, 999.99)]
    public decimal? Knee { get; set; }

    [Range(0, 999.99)]
    public decimal? InseamLength { get; set; }

    [Range(0, 999.99)]
    public decimal? OutseamLength { get; set; }

    [Range(0, 999.99)]
    public decimal? Height { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}
