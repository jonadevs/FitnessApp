using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FitnessApp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutTypeDateAndLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Workouts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Workouts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Workouts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "Length", "Type" },
                values: new object[] { new DateTime(2023, 9, 14, 17, 0, 0, 0, DateTimeKind.Unspecified), 90, 0 });

            migrationBuilder.UpdateData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "Length", "Type" },
                values: new object[] { new DateTime(2023, 9, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), 120, 0 });

            migrationBuilder.UpdateData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "Length", "Type" },
                values: new object[] { new DateTime(2023, 9, 20, 17, 0, 0, 0, DateTimeKind.Unspecified), 120, 0 });

            migrationBuilder.InsertData(
                table: "Workouts",
                columns: new[] { "Id", "Date", "Length", "Name", "Type" },
                values: new object[,]
                {
                    { 4, new DateTime(2023, 12, 28, 14, 0, 0, 0, DateTimeKind.Unspecified), 31, "Laufen 28/12", 1 },
                    { 5, new DateTime(2023, 12, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), 33, "Laufen 30/12", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Workouts");
        }
    }
}
