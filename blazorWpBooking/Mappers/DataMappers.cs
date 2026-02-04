using blazorWpBooking.Data;
using blazorWpBooking.Shared.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
[UseStaticMapper(typeof(SpecialDateMapper))]
public static partial class CalendarMapper
{
    // ignore CalendarSerring.Id
    [MapperIgnoreTarget(nameof(CalendarSetting.Id))]
    [MapperIgnoreSource(nameof(CalendarSettingDto.SpecialDates))]
    public static partial CalendarSetting ToEntity(this CalendarSettingDto source);

    [MapperIgnoreSource(nameof(CalendarSetting.Id))]
    [MapperIgnoreTarget(nameof(CalendarSettingDto.SpecialDates))]
    public static partial CalendarSettingDto ToDto(this CalendarSetting source);

} 

[Mapper]
public static partial class SpecialDateMapper
{
    
    [MapperIgnoreTarget(nameof(SpecialDate.Id))]
    public static partial SpecialDate ToEntity(this SpecialDateDto source);
   
    [MapperIgnoreSource(nameof(SpecialDate.Id))]
    public static partial SpecialDateDto ToDto(this SpecialDate source);

    public static partial IEnumerable<SpecialDateDto> ProjectToDto(this IEnumerable<SpecialDate> source);
} 

[Mapper]
public static partial class ScheduleMapper
{
    public static partial ScheduleDto ToDto(this blazorWpBooking.Data.Schedule source);

    public static partial IEnumerable<ScheduleDto> ProjectToDto(this IEnumerable<blazorWpBooking.Data.Schedule> source);

    // custom conversion helpers used by the generated mapper
    public static string? ConvertTimeOnlyToString(TimeOnly? t) => t?.ToString("HH:mm");

    public static string? ConvertLocationName(blazorWpBooking.Data.Location? l) => l?.Name;

    public static string? ConvertLocationColor(blazorWpBooking.Data.Location? l) => l?.Color;

    public static string? ConvertLessonTypeName(blazorWpBooking.Data.LessonType? t) => t?.Name;
}