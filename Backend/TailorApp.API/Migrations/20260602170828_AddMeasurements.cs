using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorApp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMeasurements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    MeasurementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ShopId = table.Column<int>(type: "int", nullable: false),
                    Chest = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Shoulder = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SleeveLength = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ArmHole = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Neck = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Waist = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Hip = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Thigh = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Knee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InseamLength = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OutseamLength = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.MeasurementId);
                    table.ForeignKey(
                        name: "FK_Measurements_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_CustomerId",
                table: "Measurements",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurements");
        }
    }
}
