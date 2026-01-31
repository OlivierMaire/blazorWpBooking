using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blazorWpBooking.Migrations
{
    /// <inheritdoc />
    public partial class DropLegacyHolidayTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // copy any existing legacy rows into SpecialDates (SpecialDates should already exist)
            migrationBuilder.Sql(@"INSERT OR IGNORE INTO ""SpecialDates"" (""Date"", ""Label"", ""IsDayOff"") SELECT ""Date"", ""Label"", 1 FROM ""Holidays"";");
            migrationBuilder.Sql(@"INSERT OR IGNORE INTO ""SpecialDates"" (""Date"", ""Label"", ""IsDayOff"") SELECT ""Date"", '', 0 FROM ""DayOffExceptions"";");

            // drop legacy tables if they exist
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"Holidays\";");
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"DayOffExceptions\";");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate legacy tables
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

            // optionally repopulate legacy tables from SpecialDates
            migrationBuilder.Sql(@"INSERT OR IGNORE INTO ""Holidays"" (""Date"", ""Label"") SELECT ""Date"", ""Label"" FROM ""SpecialDates"" WHERE ""IsDayOff"" = 1;");
            migrationBuilder.Sql(@"INSERT OR IGNORE INTO ""DayOffExceptions"" (""Date"", ""DayOfWeek"") SELECT ""Date"", 0 FROM ""SpecialDates"" WHERE ""IsDayOff"" = 0;");
        }
    }
}
