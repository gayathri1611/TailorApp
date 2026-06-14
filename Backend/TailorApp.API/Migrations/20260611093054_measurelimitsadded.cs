using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TailorApp.API.Migrations
{
    /// <inheritdoc />
    public partial class measurelimitsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NameValues",
                columns: new[] { "NameValueId", "Category", "IsActive", "Label", "SortOrder", "Value" },
                values: new object[,]
                {
                    { 50, "MeasurementLimit", true, "Chest (20–200)", 1, "chest:20:200" },
                    { 51, "MeasurementLimit", true, "Shoulder (10–100)", 2, "shoulder:10:100" },
                    { 52, "MeasurementLimit", true, "Sleeve Length (10–150)", 3, "sleevelength:10:150" },
                    { 53, "MeasurementLimit", true, "Arm Hole (10–100)", 4, "armhole:10:100" },
                    { 54, "MeasurementLimit", true, "Neck (10–80)", 5, "neck:10:80" },
                    { 55, "MeasurementLimit", true, "Waist (20–200)", 6, "waist:20:200" },
                    { 56, "MeasurementLimit", true, "Hip (20–200)", 7, "hip:20:200" },
                    { 57, "MeasurementLimit", true, "Thigh (10–150)", 8, "thigh:10:150" },
                    { 58, "MeasurementLimit", true, "Knee (10–120)", 9, "knee:10:120" },
                    { 59, "MeasurementLimit", true, "Inseam (10–200)", 10, "inseamlength:10:200" },
                    { 60, "MeasurementLimit", true, "Outseam (10–250)", 11, "outseamlength:10:250" },
                    { 61, "MeasurementLimit", true, "Height (30–300)", 12, "height:30:300" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NameValues",
                keyColumn: "NameValueId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "NameValues",
                keyColumn: "NameValueId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "NameValues",
                keyColumn: "NameValueId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "NameValues",
                keyColumn: "NameValueId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "NameValues",
                keyColumn: "NameValueId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "NameValues",
                keyColumn: "NameValueId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "NameValues",
                keyColumn: "NameValueId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "NameValues",
                keyColumn: "NameValueId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "NameValues",
                keyColumn: "NameValueId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "NameValues",
                keyColumn: "NameValueId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "NameValues",
                keyColumn: "NameValueId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "NameValues",
                keyColumn: "NameValueId",
                keyValue: 61);
        }
    }
}
