using System.ComponentModel.DataAnnotations;

namespace blazorWpBooking.Data;

public class Level
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public int? GroupId { get; set; }

    public Group? Group { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
