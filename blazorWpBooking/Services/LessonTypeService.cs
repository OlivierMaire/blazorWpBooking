using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using blazorWpBooking.Data;

namespace blazorWpBooking.Services
{
    public class LessonTypeService
    {
        private readonly ApplicationDbContext _db;

        public LessonTypeService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<LessonType>> GetTypesAsync()
        {
            return await _db.LessonTypes.AsNoTracking().ToListAsync();
        }

        public async Task<LessonType?> GetTypeAsync(int id)
        {
            return await _db.LessonTypes.FindAsync(id);
        }

        public async Task<LessonType> CreateTypeAsync(LessonType type)
        {
            _db.LessonTypes.Add(type);
            await _db.SaveChangesAsync();
            return type;
        }

        public async Task UpdateTypeAsync(LessonType type)
        {
            _db.LessonTypes.Update(type);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteTypeAsync(int id)
        {
            var t = await _db.LessonTypes.FindAsync(id);
            if (t != null)
            {
                _db.LessonTypes.Remove(t);
                await _db.SaveChangesAsync();
            }
        }
    }
}