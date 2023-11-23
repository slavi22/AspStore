using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AspStore.Migrations
{
    /// <inheritdoc />
    public partial class Products : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d89d2e6b-fd60-4c82-ba7f-a9c7949040d8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "de5daed4-865b-41d8-b61c-b184b6c7dba5");

            migrationBuilder.CreateTable(
                name: "ProductsCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    ProductImageId = table.Column<int>(type: "int", nullable: false),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductsCategory_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductsCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductsImages_ProductImageId",
                        column: x => x.ProductImageId,
                        principalTable: "ProductsImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d3fa81b4-2576-4206-8217-433a95c72d12", null, "Admin", "ADMIN" },
                    { "e27ab5ef-a7d3-4177-823e-eef04193922f", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "ProductsCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "CPU" },
                    { 2, "GPU" },
                    { 3, "RAM" },
                    { 4, "Motherboard" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductImageId",
                table: "Products",
                column: "ProductImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductsCategory");

            migrationBuilder.DropTable(
                name: "ProductsImages");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3fa81b4-2576-4206-8217-433a95c72d12");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e27ab5ef-a7d3-4177-823e-eef04193922f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d89d2e6b-fd60-4c82-ba7f-a9c7949040d8", null, "Admin", "ADMIN" },
                    { "de5daed4-865b-41d8-b61c-b184b6c7dba5", null, "User", "USER" }
                });
        }
    }
}
