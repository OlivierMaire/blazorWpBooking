using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using blazorWpBooking.Data;

namespace blazorWpBooking.Services
{
    public class ScheduleService
    {
        private readonly ApplicationDbContext _db;

        public ScheduleService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Schedule>> GetSchedulesAsync()
        {
            return await _db.Schedules
                .Include(s => s.Location)
                .Include(s => s.LessonType)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Schedule?> GetScheduleAsync(int id)
        {
            return await _db.Schedules.FindAsync(id);
        }

        public async Task<Schedule> CreateScheduleAsync(Schedule schedule)
        {
            _db.Schedules.Add(schedule);
            await _db.SaveChangesAsync();
            return schedule;
        }

        public async Task UpdateScheduleAsync(Schedule schedule)
        {
            _db.Schedules.Update(schedule);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteScheduleAsync(int id)
        {
            var s = await _db.Schedules.FindAsync(id);
            if (s != null)
            {
                _db.Schedules.Remove(s);
                await _db.SaveChangesAsync();
            }
        }
    }
}