using System.ComponentModel.DataAnnotations.Schema;

namespace TailorApp.API.Models;

public class Order
{
    public int OrderId { get; set; }
    public string OrderCode { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int ShopId { get; set; }
    public int? MeasurementId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveryDate { get; set; }

    public string Status { get; set; } = "Pending";

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; } = 0;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Discount { get; set; } = 0;

    [Column(TypeName = "decimal(10,2)")]
    public decimal FinalAmount { get; set; } = 0;

    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Customer? Customer { get; set; }
    public Measurement? Measurement { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}