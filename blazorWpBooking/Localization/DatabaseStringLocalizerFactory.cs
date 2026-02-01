using Microsoft.Extensions.Localization;
using blazorWpBooking.Services;

namespace blazorWpBooking.Localization;

public class DatabaseStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly LocalizationService _localizationService;

    public DatabaseStringLocalizerFactory(LocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        var localizerType = typeof(DatabaseStringLocalizer<>).MakeGenericType(resourceSource);
        return (IStringLocalizer)Activator.CreateInstance(localizerType, _localizationService)!;
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        // For non-generic case, use object as the type parameter
        return new DatabaseStringLocalizer<object>(_localizationService);
    }
}