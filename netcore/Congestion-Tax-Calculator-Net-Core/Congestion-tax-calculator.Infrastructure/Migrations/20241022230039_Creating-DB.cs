using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Congestion_tax_calculator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreatingDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CityTollRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityTollRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityTollRules_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "ID", "Description", "Name" },
                values: new object[] { 1, "", "Gothenburg" });

            migrationBuilder.InsertData(
                table: "CityTollRules",
                columns: new[] { "Id", "Amount", "CityId", "From", "To" },
                values: new object[,]
                {
                    { 1, 8, 1, "06:00", "06:29" },
                    { 2, 13, 1, "06:30", "06:59" },
                    { 4, 18, 1, "07:00", "07:59" },
                    { 5, 13, 1, "08:00", "08:29" },
                    { 6, 8, 1, "08:30", "14:59" },
                    { 7, 13, 1, "15:00", "15:29" },
                    { 8, 18, 1, "15:30", "16:59" },
                    { 9, 13, 1, "17:00", "17:59" },
                    { 10, 8, 1, "18:00", "18:29" },
                    { 11, 0, 1, "18:30", "05:59" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityTollRules_CityId",
                table: "CityTollRules",
                column: "CityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CityTollRules");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
