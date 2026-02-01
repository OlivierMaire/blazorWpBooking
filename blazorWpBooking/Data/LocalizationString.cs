using System.ComponentModel.DataAnnotations;

namespace blazorWpBooking.Data;

public class LocalizationString
{
    public int Id { get; set; }

    [Required]
    [StringLength(10)]
    public string Culture { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Key { get; set; } = string.Empty;

    [Required]
    public string Value { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}