using System;
using System.ComponentModel.DataAnnotations;

namespace blazorWpBooking.Data;

public class DayOffException
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    // DayOfWeek stored as int (0=Sunday..6=Saturday)
    public int DayOfWeek { get; set; }
}
