using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorApp.API.Migrations
{
    /// <inheritdoc />
    public partial class AlignMeasurementFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ankle",
                table: "Measurements");

            migrationBuilder.RenameColumn(
                name: "OutSeam",
                table: "Measurements",
                newName: "OutseamLength");

            migrationBuilder.RenameColumn(
                name: "Length",
                table: "Measurements",
                newName: "InseamLength");

            migrationBuilder.RenameColumn(
                name: "Inseam",
                table: "Measurements",
                newName: "Hip");

            migrationBuilder.RenameColumn(
                name: "Hips",
                table: "Measurements",
                newName: "Height");

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Measurements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Measurements");

            migrationBuilder.RenameColumn(
                name: "OutseamLength",
                table: "Measurements",
                newName: "OutSeam");

            migrationBuilder.RenameColumn(
                name: "InseamLength",
                table: "Measurements",
                newName: "Length");

            migrationBuilder.RenameColumn(
                name: "Hip",
                table: "Measurements",
                newName: "Inseam");

            migrationBuilder.RenameColumn(
                name: "Height",
                table: "Measurements",
                newName: "Hips");

            migrationBuilder.AddColumn<decimal>(
                name: "Ankle",
                table: "Measurements",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
