using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using blazorWpBooking.Data;

namespace blazorWpBooking.Services
{
    public class LessonService
    {
        private readonly ApplicationDbContext _db;

        public LessonService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Lesson>> GetAllLessonsAsync()
        {
            return await _db.Lessons.Include(l => l.Type).Include(l => l.Schedule).AsNoTracking().ToListAsync();
        }

        public async Task<Lesson?> GetLessonAsync(int id)
        {
            return await _db.Lessons.FindAsync(id);
        }

        public async Task<Lesson> CreateAsync(Lesson lesson)
        {
            _db.Lessons.Add(lesson);
            await _db.SaveChangesAsync();
            return lesson;
        }

        public async Task UpdateAsync(Lesson lesson)
        {
            _db.Lessons.Update(lesson);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var l = await _db.Lessons.FindAsync(id);
            if (l != null)
            {
                _db.Lessons.Remove(l);
                await _db.SaveChangesAsync();
            }
        }
    }
}
