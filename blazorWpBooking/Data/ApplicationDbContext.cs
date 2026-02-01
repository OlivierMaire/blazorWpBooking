using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace blazorWpBooking.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<Lesson> Lessons { get; set; } = null!;
	public DbSet<LessonType> LessonTypes { get; set; } = null!;
	public DbSet<Schedule> Schedules { get; set; } = null!;
	public DbSet<Location> Locations { get; set; } = null!;
	public DbSet<Holiday> Holidays { get; set; } = null!;
	public DbSet<CalendarSetting> CalendarSettings { get; set; } = null!;
	public DbSet<DayOffException> DayOffExceptions { get; set; } = null!;
	public DbSet<SpecialDate> SpecialDates { get; set; } = null!;
	public DbSet<Level> Levels { get; set; } = null!;
	public DbSet<Group> Groups { get; set; } = null!;
	public DbSet<LocalizationString> LocalizationStrings { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Configure relationships if needed
		modelBuilder.Entity<Lesson>()
			.HasOne(l => l.Type)
			.WithMany()
			.HasForeignKey(l => l.TypeId)
			.OnDelete(DeleteBehavior.SetNull);

		modelBuilder.Entity<Lesson>()
			.HasOne(l => l.Schedule)
			.WithMany()
			.HasForeignKey(l => l.ScheduleId)
			.OnDelete(DeleteBehavior.SetNull);

		modelBuilder.Entity<Level>()
			.HasOne(l => l.Group)
			.WithMany()
			.HasForeignKey(l => l.GroupId)
			.OnDelete(DeleteBehavior.SetNull);

		modelBuilder.Entity<Schedule>()
			.HasOne(s => s.Location)
			.WithMany()
			.HasForeignKey(s => s.LocationId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}
