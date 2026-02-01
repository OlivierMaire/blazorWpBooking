namespace blazorWpBooking.Shared.Models;

public class CalendarSetting
{
    public int Id { get; set; }

    public bool SundaysOff { get; set; }

    public bool SaturdaysOff { get; set; }

    // true = week starts on Sunday, false = week starts on Monday
    public bool WeekStartsOnSunday { get; set; } = true;
}