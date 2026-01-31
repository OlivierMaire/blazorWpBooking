using System;
using System.ComponentModel.DataAnnotations;

namespace blazorWpBooking.Data;

public class Location
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    [MaxLength(7)]
    public string? Color { get; set; }
}