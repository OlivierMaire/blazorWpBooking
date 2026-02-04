namespace blazorWpBooking.Shared.Models;

public class CalendarSettingDto
{
    public bool SundaysOff { get; set; }

    public bool SaturdaysOff { get; set; }

    // true = week starts on Sunday, false = week starts on Monday
    public bool WeekStartsOnSunday { get; set; } = true;

    public List<SpecialDateDto> SpecialDates { get; set; } = new();
}

public class SpecialDateDto
{
    public DateTime Date { get; set; }

    public string? Label { get; set; }

    // true = explicitly a day off (muted); false = explicitly a day on (override off)
    public bool IsDayOff { get; set; }
}