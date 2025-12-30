using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FoodTruckApp.Migrations
{
    /// <inheritdoc />
    public partial class Alergenes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MealType",
                table: "RestaurantItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AlergenesTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlergenNum = table.Column<int>(type: "integer", nullable: false),
                    AlergenDescription = table.Column<string>(type: "text", nullable: false),
                    AlergenPicture = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlergenesTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "restItemAlergens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RestaurantItemId = table.Column<int>(type: "integer", nullable: false),
                    AlergenesTypesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restItemAlergens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_restItemAlergens_AlergenesTypes_AlergenesTypesId",
                        column: x => x.AlergenesTypesId,
                        principalTable: "AlergenesTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_restItemAlergens_RestaurantItems_RestaurantItemId",
                        column: x => x.RestaurantItemId,
                        principalTable: "RestaurantItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_restItemAlergens_AlergenesTypesId",
                table: "restItemAlergens",
                column: "AlergenesTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_restItemAlergens_RestaurantItemId",
                table: "restItemAlergens",
                column: "RestaurantItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "restItemAlergens");

            migrationBuilder.DropTable(
                name: "AlergenesTypes");

            migrationBuilder.DropColumn(
                name: "MealType",
                table: "RestaurantItems");
        }
    }
}
