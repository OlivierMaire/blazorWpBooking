using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blazorWpBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddWeekStartToCalendarSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WeekStartsOnSunday",
                table: "CalendarSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeekStartsOnSunday",
                table: "CalendarSettings");
        }
    }
}
