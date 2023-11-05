using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FitnessApp.API.Migrations
{
    /// <inheritdoc />
    public partial class FitnessAppApiCleanDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Intensity = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    WorkoutId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Workouts",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Training 14/09" },
                    { 2, "Training 17/09" },
                    { 3, "Training 20/09" }
                });

            migrationBuilder.InsertData(
                table: "Sets",
                columns: new[] { "Id", "Intensity", "Name", "WorkoutId" },
                values: new object[,]
                {
                    { 1, "Easy", "Triceps pulldown", 1 },
                    { 2, "Hard", "Shoulder press", 1 },
                    { 3, "Easy", "Bicep curl", 2 },
                    { 4, "Medium", "Chest fly", 2 },
                    { 5, "Easy", "Hamstring curl", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sets_WorkoutId",
                table: "Sets",
                column: "WorkoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropTable(
                name: "Workouts");
        }
    }
}
