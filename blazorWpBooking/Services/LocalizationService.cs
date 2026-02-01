using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using blazorWpBooking.Data;

namespace blazorWpBooking.Services;

public class LocalizationService
{
    private readonly ApplicationDbContext _db;

    public LocalizationService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<LocalizationString>> GetAllStringsAsync()
    {
        return await _db.LocalizationStrings.OrderBy(x => x.Key).ThenBy(x => x.Culture).ToListAsync();
    }

    public async Task<List<LocalizationString>> GetStringsByCultureAsync(string culture)
    {
        return await _db.LocalizationStrings.Where(x => x.Culture == culture).OrderBy(x => x.Key).ToListAsync();
    }

    public async Task<LocalizationString?> GetStringAsync(string culture, string key)
    {
        return await _db.LocalizationStrings.FirstOrDefaultAsync(x => x.Culture == culture && x.Key == key);
    }

    public async Task<LocalizationString> AddOrUpdateStringAsync(string culture, string key, string value)
    {
        var existing = await GetStringAsync(culture, key);
        if (existing != null)
        {
            existing.Value = value;
            existing.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return existing;
        }
        else
        {
            var newString = new LocalizationString
            {
                Culture = culture,
                Key = key,
                Value = value
            };
            _db.LocalizationStrings.Add(newString);
            await _db.SaveChangesAsync();
            return newString;
        }
    }

    public async Task DeleteStringAsync(int id)
    {
        var str = await _db.LocalizationStrings.FindAsync(id);
        if (str != null)
        {
            _db.LocalizationStrings.Remove(str);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<string>> GetAvailableCulturesAsync()
    {
        return await _db.LocalizationStrings.Select(x => x.Culture).Distinct().OrderBy(x => x).ToListAsync();
    }

    public async Task<List<string>> GetAvailableKeysAsync()
    {
        return await _db.LocalizationStrings.Select(x => x.Key).Distinct().OrderBy(x => x).ToListAsync();
    }

    public async Task SeedFromResxAsync()
    {
        // Seed English strings
        var englishStrings = new Dictionary<string, string>
        {
            ["Calendar"] = "Calendar",
            ["CalendarSettings"] = "Calendar Settings",
            ["SaturdaysOff"] = "Saturdays off",
            ["SundaysOff"] = "Sundays off",
            ["WeekStartsOn"] = "Week starts on:",
            ["Holidays"] = "Holidays",
            ["Add"] = "Add",
            ["DayOff"] = "Day off",
            ["Home"] = "Home",
            ["HelloWorld"] = "Hello, world!",
            ["WelcomeMessage"] = "Welcome to your new app.",
            ["Weather"] = "Weather",
            ["WeatherDescription"] = "This component demonstrates showing data.",
            ["Loading"] = "Loading...",
            ["Date"] = "Date",
            ["TempC"] = "Temp. (C)",
            ["TempF"] = "Temp. (F)",
            ["Summary"] = "Summary",
            ["TemperatureCelsius"] = "Temperature in Celsius",
            ["TemperatureFahrenheit"] = "Temperature in Fahrenheit",
            ["Lessons"] = "Lessons",
            ["AddLesson"] = "Add Lesson",
            ["Name"] = "Name",
            ["Type"] = "Type",
            ["Schedule"] = "Schedule",
            ["None"] = "none",
            ["New"] = "New",
            ["NewTypeName"] = "New Type Name",
            ["Create"] = "Create",
            ["Cancel"] = "Cancel",
            ["NewScheduleName"] = "New Schedule Name",
            ["TimeSlotFrom"] = "Time Slot From",
            ["TimeSlotTo"] = "Time Slot To",
            ["DaysOfWeek"] = "Days of Week",
            ["IsRecurring"] = "Is Recurring",
            ["IsPublished"] = "Is Published",
            ["Published"] = "Published",
            ["Created"] = "Created",
            ["Updated"] = "Updated",
            ["Save"] = "Save",
            ["LessonTypes"] = "Lesson Types",
            ["AddType"] = "Add Type",
            ["Yes"] = "Yes",
            ["No"] = "No",
            ["Schedules"] = "Schedules",
            ["AddSchedule"] = "Add Schedule",
            ["Locations"] = "Locations",
            ["AddLocation"] = "Add Location",
            ["Address"] = "Address",
            ["Language"] = "Language",
            ["English"] = "English",
            ["French"] = "French",
            ["Japanese"] = "Japanese"
        };

        foreach (var kvp in englishStrings)
        {
            await AddOrUpdateStringAsync("en-US", kvp.Key, kvp.Value);
        }

        // Seed French strings
        var frenchStrings = new Dictionary<string, string>
        {
            ["Calendar"] = "Calendrier",
            ["CalendarSettings"] = "Paramètres du Calendrier",
            ["SaturdaysOff"] = "Samedis fermés",
            ["SundaysOff"] = "Dimanches fermés",
            ["WeekStartsOn"] = "La semaine commence le :",
            ["Holidays"] = "Jours fériés",
            ["Add"] = "Ajouter",
            ["DayOff"] = "Jour de congé",
            ["Home"] = "Accueil",
            ["HelloWorld"] = "Bonjour le monde !",
            ["WelcomeMessage"] = "Bienvenue dans votre nouvelle application.",
            ["Weather"] = "Météo",
            ["WeatherDescription"] = "Ce composant démontre l'affichage des données.",
            ["Loading"] = "Chargement...",
            ["Date"] = "Date",
            ["TempC"] = "Temp. (C)",
            ["TempF"] = "Temp. (F)",
            ["Summary"] = "Résumé",
            ["TemperatureCelsius"] = "Température en Celsius",
            ["TemperatureFahrenheit"] = "Température en Fahrenheit",
            ["Lessons"] = "Leçons",
            ["AddLesson"] = "Ajouter une leçon",
            ["Name"] = "Nom",
            ["Type"] = "Type",
            ["Schedule"] = "Horaire",
            ["None"] = "aucun",
            ["New"] = "Nouveau",
            ["NewTypeName"] = "Nouveau nom de type",
            ["Create"] = "Créer",
            ["Cancel"] = "Annuler",
            ["NewScheduleName"] = "Nouveau nom d'horaire",
            ["TimeSlotFrom"] = "Créneau horaire de",
            ["TimeSlotTo"] = "Créneau horaire à",
            ["DaysOfWeek"] = "Jours de la semaine",
            ["IsRecurring"] = "Est récurrent",
            ["IsPublished"] = "Est publié",
            ["Published"] = "Publié",
            ["Created"] = "Créé",
            ["Updated"] = "Mis à jour",
            ["Save"] = "Enregistrer",
            ["LessonTypes"] = "Types de leçons",
            ["AddType"] = "Ajouter un type",
            ["Yes"] = "Oui",
            ["No"] = "Non",
            ["Schedules"] = "Horaires",
            ["AddSchedule"] = "Ajouter un horaire",
            ["Locations"] = "Lieux",
            ["AddLocation"] = "Ajouter un lieu",
            ["Address"] = "Adresse",
            ["Language"] = "Langue",
            ["English"] = "Anglais",
            ["French"] = "Français",
            ["Japanese"] = "Japonais"
        };

        foreach (var kvp in frenchStrings)
        {
            await AddOrUpdateStringAsync("fr-FR", kvp.Key, kvp.Value);
        }

        // Seed Japanese strings
        var japaneseStrings = new Dictionary<string, string>
        {
            ["Calendar"] = "カレンダー",
            ["CalendarSettings"] = "カレンダー設定",
            ["SaturdaysOff"] = "土曜日休み",
            ["SundaysOff"] = "日曜日休み",
            ["WeekStartsOn"] = "週の始まり:",
            ["Holidays"] = "祝日",
            ["Add"] = "追加",
            ["DayOff"] = "休日",
            ["Home"] = "ホーム",
            ["HelloWorld"] = "こんにちは、世界！",
            ["WelcomeMessage"] = "新しいアプリへようこそ。",
            ["Weather"] = "天気",
            ["WeatherDescription"] = "このコンポーネントはデータの表示を示しています。",
            ["Loading"] = "読み込み中...",
            ["Date"] = "日付",
            ["TempC"] = "温度 (°C)",
            ["TempF"] = "温度 (°F)",
            ["Summary"] = "要約",
            ["TemperatureCelsius"] = "摂氏温度",
            ["TemperatureFahrenheit"] = "華氏温度",
            ["Lessons"] = "レッスン",
            ["AddLesson"] = "レッスンを追加",
            ["Name"] = "名前",
            ["Type"] = "タイプ",
            ["Schedule"] = "スケジュール",
            ["None"] = "なし",
            ["New"] = "新規",
            ["NewTypeName"] = "新しいタイプ名",
            ["Create"] = "作成",
            ["Cancel"] = "キャンセル",
            ["NewScheduleName"] = "新しいスケジュール名",
            ["TimeSlotFrom"] = "開始時間",
            ["TimeSlotTo"] = "終了時間",
            ["DaysOfWeek"] = "曜日",
            ["IsRecurring"] = "繰り返し",
            ["IsPublished"] = "公開済み",
            ["Published"] = "公開済み",
            ["Created"] = "作成日",
            ["Updated"] = "更新日",
            ["Save"] = "保存",
            ["LessonTypes"] = "レッスンタイプ",
            ["AddType"] = "タイプを追加",
            ["Yes"] = "はい",
            ["No"] = "いいえ",
            ["Schedules"] = "スケジュール",
            ["AddSchedule"] = "スケジュールを追加",
            ["Locations"] = "場所",
            ["AddLocation"] = "場所を追加",
            ["Address"] = "住所",
            ["Language"] = "言語",
            ["English"] = "英語",
            ["French"] = "フランス語",
            ["Japanese"] = "日本語"
        };

        foreach (var kvp in japaneseStrings)
        {
            await AddOrUpdateStringAsync("ja-JP", kvp.Key, kvp.Value);
        }
    }
}