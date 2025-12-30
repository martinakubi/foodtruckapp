using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FoodTruckApp.Migrations
{
    /// <inheritdoc />
    public partial class Orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemsOfOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NewOrderId = table.Column<int>(type: "integer", nullable: false),
                    RestaurantItemId = table.Column<int>(type: "integer", nullable: false),
                    PricePerOne = table.Column<double>(type: "double precision", nullable: false),
                    NumOfDishes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsOfOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemsOfOrders_NewOrders_NewOrderId",
                        column: x => x.NewOrderId,
                        principalTable: "NewOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemsOfOrders_RestaurantItems_RestaurantItemId",
                        column: x => x.RestaurantItemId,
                        principalTable: "RestaurantItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemsOfOrders_NewOrderId",
                table: "ItemsOfOrders",
                column: "NewOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsOfOrders_RestaurantItemId",
                table: "ItemsOfOrders",
                column: "RestaurantItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemsOfOrders");

            migrationBuilder.DropTable(
                name: "NewOrders");
        }
    }
}
