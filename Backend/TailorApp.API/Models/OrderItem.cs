using System.ComponentModel.DataAnnotations.Schema;

namespace TailorApp.API.Models;

public class OrderItem
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }

    public string GarmentType { get; set; } = string.Empty;
    // e.g. Shirt, Pant, Blouse, Saree, Suit

    public string? Description { get; set; }

    public int Quantity { get; set; } = 1;

    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; } = 0;

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; } = 0;

    public string? FabricDetails { get; set; }
    public string? SpecialInstructions { get; set; }

    public Order? Order { get; set; }
}