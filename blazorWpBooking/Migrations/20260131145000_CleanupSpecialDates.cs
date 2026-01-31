using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blazorWpBooking.Migrations
{
    public partial class CleanupSpecialDates_Old : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Migrate existing holidays and exceptions into SpecialDates (SpecialDates should already exist from a previous migration)
            migrationBuilder.Sql(@"INSERT OR IGNORE INTO ""SpecialDates"" (""Date"", ""Label"", ""IsDayOff"") SELECT ""Date"", ""Label"", 1 FROM ""Holidays"";");
            migrationBuilder.Sql(@"INSERT OR IGNORE INTO ""SpecialDates"" (""Date"", ""Label"", ""IsDayOff"") SELECT ""Date"", '', 0 FROM ""DayOffExceptions"";");

            // Remove legacy tables
            migrationBuilder.DropTable(name: "Holidays");
            migrationBuilder.DropTable(name: "DayOffExceptions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate legacy tables
            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Label = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayOffExceptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DayOfWeek = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayOffExceptions", x => x.Id);
                });

            // Copy back from SpecialDates where IsDayOff = true -> Holidays
            migrationBuilder.Sql(@"INSERT INTO ""Holidays"" (""Date"", ""Label"") SELECT ""Date"", ""Label"" FROM ""SpecialDates"" WHERE ""IsDayOff"" = 1;");

            // Copy back where IsDayOff = false -> DayOffExceptions (DayOfWeek default to 0)
            migrationBuilder.Sql(@"INSERT INTO ""DayOffExceptions"" (""Date"", ""DayOfWeek"") SELECT ""Date"", 0 FROM ""SpecialDates"" WHERE ""IsDayOff"" = 0;");

            migrationBuilder.DropTable(name: "SpecialDates");
        }
    }
}
