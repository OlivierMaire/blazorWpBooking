// calendar api endpoint
using FastEndpoints;
using blazorWpBooking.Services;
using blazorWpBooking.Shared.Models;

namespace blazorWpBooking.Api.Endpoints;

public class CalendarEndpoint(CalendarService calendarService) : EndpointWithoutRequest<CalendarSettingDto>
{

    public override void Configure()
    {
        Get(Shared.EndpointsUrl.Calendar.GetCalendarSettings);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var settings = await calendarService.GetSettingsAsync();
        var model = settings.ToDto();

        model.SpecialDates = (await calendarService.GetSpecialDatesAsync()).ProjectToDto().ToList();



        await Send.OkAsync(model, ct);
    }
}

public class CalendarAvailabilityEndpoint : EndpointWithoutRequest<List<CalendarAvailabilityDto>>    
{
    public override void Configure()
    {
        Get(Shared.EndpointsUrl.Calendar.GetCalendarAvailability);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = new CalendarAvailabilityDto
        {
            AvailableDates = new List<DateTime>
            {
                DateTime.Today.AddDays(1),
                DateTime.Today.AddDays(3),
                DateTime.Today.AddDays(5)
            }
        };

        await Send.OkAsync(new List<Shared.Models.CalendarAvailabilityDto> { response }, ct);
    }
}