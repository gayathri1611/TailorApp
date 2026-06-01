namespace TailorApp.API.Models;

public class Shop
{
    public int ShopId { get; set; }

    public string ShopName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; }

    public string Address { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}