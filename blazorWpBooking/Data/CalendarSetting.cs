using System.ComponentModel.DataAnnotations;

namespace blazorWpBooking.Data;

public class CalendarSetting
{
    [Key]
    public int Id { get; set; }

    public bool SundaysOff { get; set; }

    public bool SaturdaysOff { get; set; }

    // true = week starts on Sunday, false = week starts on Monday
    public bool WeekStartsOnSunday { get; set; } = true;
}
