using System;
using System.ComponentModel.DataAnnotations;

namespace blazorWpBooking.Data;

public class SpecialDate
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    [MaxLength(250)]
    public string? Label { get; set; }

    // true = explicitly a day off (muted); false = explicitly a day on (override off)
    public bool IsDayOff { get; set; }
}
