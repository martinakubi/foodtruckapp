using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FoodTruckApp.Migrations
{
    /// <inheritdoc />
    public partial class AddKennitala : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kennitalas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NewOrderId = table.Column<int>(type: "integer", nullable: false),
                    KennitalaNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    KennitalaName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kennitalas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kennitalas_NewOrders_NewOrderId",
                        column: x => x.NewOrderId,
                        principalTable: "NewOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kennitalas_NewOrderId",
                table: "Kennitalas",
                column: "NewOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kennitalas");
        }
    }
}
