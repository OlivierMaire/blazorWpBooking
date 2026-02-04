using FastEndpoints;
using blazorWpBooking.Services;
using blazorWpBooking.Shared.Models;

namespace blazorWpBooking.Api.Endpoints;

public class SchedulesEndpoint(ScheduleService scheduleService) : EndpointWithoutRequest<List<ScheduleDto>>
{
    public override void Configure()
    {
        Get(Shared.EndpointsUrl.Schedules.GetSchedules);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var schedules = await scheduleService.GetSchedulesAsync();
        var dtos = schedules.ProjectToDto().ToList();
        await Send.OkAsync(dtos, ct);
    }
}
