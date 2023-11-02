using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.API.Migrations
{
    /// <inheritdoc />
    public partial class DataSeedFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sets_Workouts_CityId",
                table: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Sets_CityId",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Sets");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_WorkoutId",
                table: "Sets",
                column: "WorkoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_Workouts_WorkoutId",
                table: "Sets",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sets_Workouts_WorkoutId",
                table: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Sets_WorkoutId",
                table: "Sets");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Sets",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CityId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CityId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 3,
                column: "CityId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 4,
                column: "CityId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 5,
                column: "CityId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Sets_CityId",
                table: "Sets",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_Workouts_CityId",
                table: "Sets",
                column: "CityId",
                principalTable: "Workouts",
                principalColumn: "Id");
        }
    }
}
