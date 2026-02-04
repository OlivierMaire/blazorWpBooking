namespace blazorWpBooking.Shared.Models;

public class ScheduleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? TimeSlotFrom { get; set; }
    public string? TimeSlotTo { get; set; }
    public bool IsRecurring { get; set; }
    public string? DaysOfWeek { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? LocationId { get; set; }
    public string? LocationName { get; set; }
    public string? LocationColor { get; set; }
    public int? LessonTypeId { get; set; }
    public string? LessonTypeName { get; set; }
    public int MaxStudents { get; set; }
}
