using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blazorWpBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPublishedAndDatesToLessonType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Lessons",
                newName: "IsPublished");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "LessonTypes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "LessonTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "LessonTypes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "LessonTypes");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Lessons",
                newName: "IsActive");
        }
    }
}
