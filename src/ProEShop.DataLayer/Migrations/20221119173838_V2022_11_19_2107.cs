using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProEShop.DataLayer.Migrations
{
    public partial class V2022_11_19_2107 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_ProductId_VariantId_GuaranteeId",
                table: "ProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_SellerId",
                table: "ProductVariants");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_SellerId_ProductId_VariantId",
                table: "ProductVariants",
                columns: new[] { "SellerId", "ProductId", "VariantId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_SellerId_ProductId_VariantId",
                table: "ProductVariants");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId_VariantId_GuaranteeId",
                table: "ProductVariants",
                columns: new[] { "ProductId", "VariantId", "GuaranteeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_SellerId",
                table: "ProductVariants",
                column: "SellerId");
        }
    }
}
