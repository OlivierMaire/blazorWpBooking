using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blazorWpBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduleStartEndDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Schedules",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Schedules",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Schedules");
        }
    }
}
