using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AspStore.Migrations
{
    /// <inheritdoc />
    public partial class Orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0a4e02a2-1ded-4c61-9b5d-bb8f38f7bfdb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "148fdd63-1afc-4068-946a-11fc09ee0f73");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0b2daa3f-2096-4e44-89d1-c4c1904b935b", null, "User", "USER" },
                    { "87297792-c19e-445b-8da9-f08899274325", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0b2daa3f-2096-4e44-89d1-c4c1904b935b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87297792-c19e-445b-8da9-f08899274325");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0a4e02a2-1ded-4c61-9b5d-bb8f38f7bfdb", null, "Admin", "ADMIN" },
                    { "148fdd63-1afc-4068-946a-11fc09ee0f73", null, "User", "USER" }
                });
        }
    }
}
