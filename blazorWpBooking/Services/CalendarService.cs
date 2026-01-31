using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using blazorWpBooking.Data;

namespace blazorWpBooking.Services
{
    public class CalendarService
    {
        private readonly ApplicationDbContext _db;

        public CalendarService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<CalendarSetting> GetSettingsAsync()
        {
            var s = await _db.CalendarSettings.FirstOrDefaultAsync();
            if (s == null)
            {
                s = new CalendarSetting { SundaysOff = false, SaturdaysOff = false, WeekStartsOnSunday = true };
                _db.CalendarSettings.Add(s);
                await _db.SaveChangesAsync();
            }
            return s;
        }

        public async Task UpdateSettingsAsync(CalendarSetting settings)
        {
            _db.CalendarSettings.Update(settings);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Holiday>> GetHolidaysAsync()
        {
            return await _db.Holidays.AsNoTracking().OrderBy(h => h.Date).ToListAsync();
        }

        public async Task<Holiday> AddHolidayAsync(Holiday h)
        {
            _db.Holidays.Add(h);
            await _db.SaveChangesAsync();
            return h;
        }

        public async Task RemoveHolidayAsync(DateTime date)
        {
            var list = await _db.Holidays.Where(x => x.Date.Date == date.Date).ToListAsync();
            if (list.Any())
            {
                _db.Holidays.RemoveRange(list);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<List<SpecialDate>> GetSpecialDatesAsync()
        {
            return await _db.SpecialDates.AsNoTracking().OrderBy(s => s.Date).ToListAsync();
        }

        public async Task<SpecialDate> AddSpecialDateAsync(SpecialDate s)
        {
            _db.SpecialDates.Add(s);
            await _db.SaveChangesAsync();
            return s;
        }

        public async Task RemoveSpecialDateAsync(DateTime date)
        {
            var list = await _db.SpecialDates.Where(x => x.Date.Date == date.Date).ToListAsync();
            if (list.Any())
            {
                _db.SpecialDates.RemoveRange(list);
                await _db.SaveChangesAsync();
            }
        }

        // If SpecialDates table is empty but legacy tables contain data, migrate them into SpecialDates.
        public async Task EnsureSpecialDatesMigratedAsync()
        {
            var any = await _db.SpecialDates.AnyAsync();
            if (any) return;

            // migrate Holidays -> SpecialDates (IsDayOff = true)
            if (await _db.Holidays.AnyAsync())
            {
                var hs = await _db.Holidays.AsNoTracking().ToListAsync();
                foreach (var h in hs)
                {
                    _db.SpecialDates.Add(new SpecialDate { Date = h.Date.Date, Label = h.Label, IsDayOff = true });
                }
            }

            // migrate DayOffExceptions -> SpecialDates (IsDayOff = false)
            if (await _db.DayOffExceptions.AnyAsync())
            {
                var ex = await _db.DayOffExceptions.AsNoTracking().ToListAsync();
                foreach (var e in ex)
                {
                    _db.SpecialDates.Add(new SpecialDate { Date = e.Date.Date, Label = string.Empty, IsDayOff = false });
                }
            }

            await _db.SaveChangesAsync();
        }
    }
}
