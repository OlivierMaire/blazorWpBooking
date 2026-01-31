using System;
using System.ComponentModel.DataAnnotations;

namespace blazorWpBooking.Data;

public class Holiday
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    [MaxLength(250)]
    public string? Label { get; set; }
}
