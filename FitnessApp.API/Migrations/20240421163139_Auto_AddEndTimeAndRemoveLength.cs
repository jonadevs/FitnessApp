using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.API.Migrations
{
    /// <inheritdoc />
    public partial class Auto_AddEndTimeAndRemoveLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Length",
                table: "Workouts");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Workouts",
                newName: "StartTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Workouts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 1,
                column: "EndTime",
                value: new DateTime(2023, 9, 14, 18, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 2,
                column: "EndTime",
                value: new DateTime(2023, 9, 17, 19, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 3,
                column: "EndTime",
                value: new DateTime(2023, 9, 20, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 4,
                column: "EndTime",
                value: new DateTime(2023, 12, 28, 14, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 5,
                column: "EndTime",
                value: new DateTime(2023, 12, 30, 12, 33, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Workouts");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Workouts",
                newName: "Date");

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Workouts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Length",
                value: 90);

            migrationBuilder.UpdateData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Length",
                value: 120);

            migrationBuilder.UpdateData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Length",
                value: 120);

            migrationBuilder.UpdateData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 4,
                column: "Length",
                value: 31);

            migrationBuilder.UpdateData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 5,
                column: "Length",
                value: 33);
        }
    }
}
