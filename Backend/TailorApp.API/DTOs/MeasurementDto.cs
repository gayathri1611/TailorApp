namespace TailorApp.API.DTOs;

public class MeasurementDto
{
    public int MeasurementId { get; set; }
    public string MeasurementCode { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int ShopId { get; set; }

    // Upper body
    public decimal? Chest { get; set; }
    public decimal? Shoulder { get; set; }
    public decimal? SleeveLength { get; set; }
    public decimal? ArmHole { get; set; }
    public decimal? Neck { get; set; }

    // Lower body
    public decimal? Waist { get; set; }
    public decimal? Hip { get; set; }
    public decimal? Thigh { get; set; }
    public decimal? Knee { get; set; }
    public decimal? InseamLength { get; set; }
    public decimal? OutseamLength { get; set; }

    // Full body
    public decimal? Height { get; set; }

    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class CreateMeasurementDto
{
    public int CustomerId { get; set; }
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
}

public class UpdateMeasurementDto
{
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
}