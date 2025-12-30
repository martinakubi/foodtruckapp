using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodTruckApp.Migrations
{
    /// <inheritdoc />
    public partial class StornoNotePayMet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "NewOrders",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StornoNote",
                table: "NewOrders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "NewOrders");

            migrationBuilder.DropColumn(
                name: "StornoNote",
                table: "NewOrders");
        }
    }
}
