using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blazorWpBooking.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLessonsAddScheduleProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.AddColumn<int>(
                name: "LessonTypeId",
                table: "Schedules",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxStudents",
                table: "Schedules",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_LessonTypeId",
                table: "Schedules",
                column: "LessonTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_LessonTypes_LessonTypeId",
                table: "Schedules",
                column: "LessonTypeId",
                principalTable: "LessonTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_LessonTypes_LessonTypeId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_LessonTypeId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "LessonTypeId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "MaxStudents",
                table: "Schedules");

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: true),
                    TypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsPublished = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_LessonTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "LessonTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Lessons_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ScheduleId",
                table: "Lessons",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TypeId",
                table: "Lessons",
                column: "TypeId");
        }
    }
}
