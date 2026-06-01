namespace TailorApp.API.Models;

public class Customer
{
    public int CustomerId { get; set; }

    public int ShopId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Shop? Shop { get; set; }
}