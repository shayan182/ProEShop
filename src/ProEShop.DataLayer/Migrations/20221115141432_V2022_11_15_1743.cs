using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProEShop.DataLayer.Migrations
{
    public partial class V2022_11_15_1743 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guarantees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MonthCount = table.Column<byte>(type: "tinyint", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SellerId = table.Column<long>(type: "bigint", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guarantees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guarantees_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guarantees_SellerId",
                table: "Guarantees",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Guarantees_Title",
                table: "Guarantees",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guarantees");
        }
    }
}
