// calendar api endpoint
using FastEndpoints;
using blazorWpBooking.Services;
using blazorWpBooking.Data;

namespace blazorWpBooking.Api.Endpoints;

public class CalendarEndpoint(CalendarService calendarService) : EndpointWithoutRequest<CalendarSetting>
{

    public override void Configure()
    {
        Get("/api/calendar/settings");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var settings = await calendarService.GetSettingsAsync();

        await Send.OkAsync(settings, ct);
    }
}