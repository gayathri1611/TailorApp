using System.ComponentModel.DataAnnotations;

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
    public string? FabricDetails { get; set; }
    public string? SpecialInstructions { get; set; }
}

public class CreateOrderItemDto
{
    [Required]
    [MaxLength(100)]
    public string GarmentType { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(1, 1000)]
    public int Quantity { get; set; } = 1;

    [Range(0, 9999999)]
    public decimal UnitPrice { get; set; }

    [MaxLength(500)]
    public string? FabricDetails { get; set; }

    [MaxLength(1000)]
    public string? SpecialInstructions { get; set; }
}

public class OrderDto
{
    public int OrderId { get; set; }
    public string OrderCode { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int ShopId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
}

public class CreateOrderDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "A valid customer is required.")]
    public int CustomerId { get; set; }

    [Range(1, int.MaxValue)]
    public int ShopId { get; set; }

    public DateTime? DeliveryDate { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one order item is required.")]
    public List<CreateOrderItemDto> OrderItems { get; set; } = new();
}

public class UpdateOrderDto
{
    public DateTime? DeliveryDate { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Notes { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one order item is required.")]
    public List<CreateOrderItemDto> OrderItems { get; set; } = new();
}

public class UpdateOrderStatusDto
{
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;
}
