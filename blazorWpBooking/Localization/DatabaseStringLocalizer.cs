using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using blazorWpBooking.Services;

namespace blazorWpBooking.Localization;

public class DatabaseStringLocalizer<T> : IStringLocalizer<T>
{
    private readonly LocalizationService _localizationService;
    private readonly CultureInfo _culture;

    public DatabaseStringLocalizer(LocalizationService localizationService)
    {
        _localizationService = localizationService;
        _culture = CultureInfo.CurrentUICulture;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value ?? name, value == null);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var format = GetString(name);
            var value = string.Format(format ?? name, arguments);
            return new LocalizedString(name, value, format == null);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        // This is a simplified implementation - in a real app you might want to cache this
        var strings = Task.Run(() => _localizationService.GetStringsByCultureAsync(_culture.Name)).Result;
        return strings.Select(s => new LocalizedString(s.Key, s.Value, false));
    }

    private string? GetString(string key)
    {
        // Try current culture first
        var localizedString = Task.Run(() => _localizationService.GetStringAsync(_culture.Name, key)).Result;
        if (localizedString != null)
        {
            return localizedString.Value;
        }

        // Try parent culture if available
        if (_culture.Parent != null && _culture.Parent != _culture)
        {
            localizedString = Task.Run(() => _localizationService.GetStringAsync(_culture.Parent.Name, key)).Result;
            if (localizedString != null)
            {
                return localizedString.Value;
            }
        }

        // Try invariant culture (en-US as fallback)
        localizedString = Task.Run(() => _localizationService.GetStringAsync("en-US", key)).Result;
        return localizedString?.Value;
    }
}