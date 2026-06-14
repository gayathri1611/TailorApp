using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TailorApp.API.Migrations
{
    /// <inheritdoc />
    public partial class addedvalidations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NameValues",
                columns: table => new
                {
                    NameValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NameValues", x => x.NameValueId);
                });

            migrationBuilder.InsertData(
                table: "NameValues",
                columns: new[] { "NameValueId", "Category", "IsActive", "Label", "SortOrder", "Value" },
                values: new object[,]
                {
                    { 1, "Unit", true, "Inch (\")", 1, "inch" },
                    { 2, "Unit", true, "Centimeter (cm)", 2, "cm" },
                    { 10, "GarmentType", true, null, 1, "Shirt" },
                    { 11, "GarmentType", true, null, 2, "Pant" },
                    { 12, "GarmentType", true, null, 3, "Blouse" },
                    { 13, "GarmentType", true, null, 4, "Saree Fall" },
                    { 14, "GarmentType", true, null, 5, "Churidar" },
                    { 15, "GarmentType", true, null, 6, "Kurti" },
                    { 16, "GarmentType", true, null, 7, "Suit" },
                    { 17, "GarmentType", true, null, 8, "Coat" },
                    { 18, "GarmentType", true, null, 9, "Frock" },
                    { 19, "GarmentType", true, null, 10, "Lehenga" },
                    { 20, "GarmentType", true, null, 11, "Salwar" },
                    { 21, "GarmentType", true, null, 12, "Jacket" },
                    { 30, "FabricType", true, null, 1, "Cotton" },
                    { 31, "FabricType", true, null, 2, "Silk" },
                    { 32, "FabricType", true, null, 3, "Linen" },
                    { 33, "FabricType", true, null, 4, "Polyester" },
                    { 34, "FabricType", true, null, 5, "Chiffon" },
                    { 35, "FabricType", true, null, 6, "Georgette" },
                    { 36, "FabricType", true, null, 7, "Velvet" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NameValues");
        }
    }
}
