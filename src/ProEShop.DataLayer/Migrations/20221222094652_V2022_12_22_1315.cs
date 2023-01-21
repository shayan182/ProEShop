using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProEShop.DataLayer.Migrations
{
    public partial class V2022_12_22_1315 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConsignmentItems_ProductVariantId",
                table: "ConsignmentItems");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentItems_ProductVariantId_ConsignmentId",
                table: "ConsignmentItems",
                columns: new[] { "ProductVariantId", "ConsignmentId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConsignmentItems_ProductVariantId_ConsignmentId",
                table: "ConsignmentItems");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentItems_ProductVariantId",
                table: "ConsignmentItems",
                column: "ProductVariantId");
        }
    }
}
