using System.ComponentModel.DataAnnotations;

namespace TailorApp.API.DTOs;

public class CreateCustomerDto
{
    public int ShopId { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }

    public string? Customercode { get; set; }

    [Required]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Address { get; set; }
}