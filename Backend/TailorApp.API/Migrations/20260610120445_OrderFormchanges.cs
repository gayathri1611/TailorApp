using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorApp.API.Migrations
{
    /// <inheritdoc />
    public partial class OrderFormchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FabricDetails",
                table: "OrderItems");

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Orders",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalAmount",
                table: "Orders",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "MeasurementId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FabricId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MeasurementId",
                table: "Orders",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_FabricId",
                table: "OrderItems",
                column: "FabricId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_FabricInventories_FabricId",
                table: "OrderItems",
                column: "FabricId",
                principalTable: "FabricInventories",
                principalColumn: "FabricId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Measurements_MeasurementId",
                table: "Orders",
                column: "MeasurementId",
                principalTable: "Measurements",
                principalColumn: "MeasurementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_FabricInventories_FabricId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Measurements_MeasurementId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_MeasurementId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_FabricId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FinalAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MeasurementId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FabricId",
                table: "OrderItems");

            migrationBuilder.AddColumn<string>(
                name: "FabricDetails",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
