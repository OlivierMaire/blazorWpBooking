using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using blazorWpBooking.Data;

namespace blazorWpBooking.Services
{
    public class LocationService
    {
        private readonly ApplicationDbContext _db;

        public LocationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Location>> GetLocationsAsync()
        {
            return await _db.Locations.AsNoTracking().ToListAsync();
        }

        public async Task<Location?> GetLocationAsync(int id)
        {
            return await _db.Locations.FindAsync(id);
        }

        public async Task<Location> CreateLocationAsync(Location location)
        {
            _db.Locations.Add(location);
            await _db.SaveChangesAsync();
            return location;
        }

        public async Task UpdateLocationAsync(Location location)
        {
            _db.Locations.Update(location);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteLocationAsync(int id)
        {
            var l = await _db.Locations.FindAsync(id);
            if (l != null)
            {
                _db.Locations.Remove(l);
                await _db.SaveChangesAsync();
            }
        }
    }
}