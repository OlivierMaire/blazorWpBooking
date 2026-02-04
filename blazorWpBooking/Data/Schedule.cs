using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace blazorWpBooking.Data
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Description { get; set; }

        // TimeSlot range
        public TimeOnly? TimeSlotFrom { get; set; }
        public TimeOnly? TimeSlotTo { get; set; }

        public bool IsRecurring { get; set; } = false;

        // Days of week as comma-separated integers: "1,3,5" (Monday=1, Tuesday=2, etc.)
        [MaxLength(20)]
        public string? DaysOfWeek { get; set; }

        public bool IsPublished { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Optional start/end dates for the schedule
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Link to Location (many schedules -> one location)
        public int? LocationId { get; set; }

        public Location? Location { get; set; }
 
        // Link to a default LessonType for this schedule
        public int? LessonTypeId { get; set; }
        public LessonType? LessonType { get; set; }

        // Maximum number of students for the schedule (0 = unlimited)
        public int MaxStudents { get; set; } = 0;

        // Helper methods for days of week
        public string GetDaysOfWeekDisplay()
        {
            if (string.IsNullOrEmpty(DaysOfWeek))
                return string.Empty;

            var dayNumbers = DaysOfWeek.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var dayNames = new List<string>();

            foreach (var dayNum in dayNumbers)
            {
                if (int.TryParse(dayNum.Trim(), out int num) && Enum.IsDefined(typeof(DayOfWeek), num))
                {
                    dayNames.Add(((DayOfWeek)num).ToString());
                }
            }

            return string.Join(", ", dayNames);
        }

        public void SetDaysOfWeek(List<DayOfWeek> days)
        {
            if (days == null || days.Count == 0)
            {
                DaysOfWeek = null;
                return;
            }

            DaysOfWeek = string.Join(",", days.Select(d => (int)d).OrderBy(x => x));
        }

        public List<DayOfWeek> GetDaysOfWeekList()
        {
            if (string.IsNullOrEmpty(DaysOfWeek))
                return new List<DayOfWeek>();

            var dayNumbers = DaysOfWeek.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var days = new List<DayOfWeek>();

            foreach (var dayNum in dayNumbers)
            {
                if (int.TryParse(dayNum.Trim(), out int num) && Enum.IsDefined(typeof(DayOfWeek), num))
                {
                    days.Add((DayOfWeek)num);
                }
            }

            return days;
        }

        // Return occurrences within a date range (inclusive). For recurring schedules, returns each date matching
        // the configured days of week between StartDate (or CreatedAt) and EndDate (or no end).
        public IEnumerable<DateTime> GetOccurrences(DateTime rangeStart, DateTime rangeEnd)
        {
            var actualStart = (StartDate ?? CreatedAt).Date;
            var actualEnd = EndDate?.Date ?? DateTime.MaxValue.Date;

            var from = rangeStart.Date > actualStart ? rangeStart.Date : actualStart;
            var to = rangeEnd.Date < actualEnd ? rangeEnd.Date : actualEnd;

            if (from > to) yield break;

            if (IsRecurring)
            {
                var days = GetDaysOfWeekList();
                if (days == null || days.Count == 0) yield break;

                for (var d = from; d <= to; d = d.AddDays(1))
                {
                    if (days.Contains(d.DayOfWeek))
                    {
                        DateTime occ = d;
                        if (TimeSlotFrom.HasValue)
                        {
                            var t = TimeSlotFrom.Value;
                            occ = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, t.Second);
                        }
                        yield return occ;
                    }
                }
            }
            else
            {
                var occDate = (StartDate ?? CreatedAt).Date;
                if (occDate >= from && occDate <= to)
                {
                    DateTime occ = occDate;
                    if (TimeSlotFrom.HasValue)
                    {
                        var t = TimeSlotFrom.Value;
                        occ = new DateTime(occDate.Year, occDate.Month, occDate.Day, t.Hour, t.Minute, t.Second);
                    }
                    yield return occ;
                }
            }
        }
    }
}
