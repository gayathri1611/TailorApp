namespace TailorApp.API.DTOs;

public class OrderItemDto
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public string GarmentType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public int? FabricId { get; set; }
    public string? FabricName { get; set; }
    public string? SpecialInstructions { get; set; }
}

public class CreateOrderItemDto
{
    public string GarmentType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public int? FabricId { get; set; }
    public string? SpecialInstructions { get; set; }
}

public class OrderDto
{
    public int OrderId { get; set; }
    public string OrderCode { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int ShopId { get; set; }
    public int? MeasurementId { get; set; }
    public string? MeasurementCode { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal FinalAmount { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
}

public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public int ShopId { get; set; }
    public int? MeasurementId { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? Notes { get; set; }
    public decimal Discount { get; set; } = 0;
    public List<CreateOrderItemDto> OrderItems { get; set; } = new();
}

public class UpdateOrderDto
{
    public int? MeasurementId { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public decimal Discount { get; set; } = 0;
    public List<CreateOrderItemDto> OrderItems { get; set; } = new();
}

public class UpdateOrderStatusDto
{
    public string Status { get; set; } = string.Empty;
}