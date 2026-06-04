using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorApp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFabricInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FabricInventories",
                columns: table => new
                {
                    FabricId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FabricCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FabricType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityInMeters = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    QuantityInItems = table.Column<int>(type: "int", nullable: false),
                    LowStockThresholdMeters = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    LowStockThresholdItems = table.Column<int>(type: "int", nullable: false),
                    PricePerMeter = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FabricInventories", x => x.FabricId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FabricInventories");
        }
    }
}
