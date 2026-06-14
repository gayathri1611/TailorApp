namespace TailorApp.API.Models;

public class NameValue
{
    public int NameValueId { get; set; }
    public string Category { get; set; } = string.Empty;
    // e.g. Unit, GarmentType, FabricType, OrderStatus

    public string Value { get; set; } = string.Empty;
    // e.g. inch, cm, Shirt, Pant, Cotton

    public string? Label { get; set; }
    // Display label (optional, falls back to Value)

    public int SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}