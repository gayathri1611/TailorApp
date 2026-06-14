namespace TailorApp.API.DTOs;

public class NameValueDto
{
    public int NameValueId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public class CreateNameValueDto
{
    public string Category { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Label { get; set; }
    public int SortOrder { get; set; } = 0;
}

public class UpdateNameValueDto
{
    public string Value { get; set; } = string.Empty;
    public string? Label { get; set; }
    public int SortOrder { get; set; }
}