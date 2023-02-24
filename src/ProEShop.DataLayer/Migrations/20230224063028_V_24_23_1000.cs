using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProEShop.DataLayer.Migrations
{
    public partial class V_24_23_1000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStocks_ProductVariants_ProductVariantId",
                table: "ProductStocks");

            migrationBuilder.RenameColumn(
                name: "count",
                table: "ProductStocks",
                newName: "Count");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "ProductVariants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductStocks_ConsignmentId",
                table: "ProductStocks",
                column: "ConsignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStocks_Consignments_ConsignmentId",
                table: "ProductStocks",
                column: "ConsignmentId",
                principalTable: "Consignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStocks_ProductVariants_ProductVariantId",
                table: "ProductStocks",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStocks_Consignments_ConsignmentId",
                table: "ProductStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductStocks_ProductVariants_ProductVariantId",
                table: "ProductStocks");

            migrationBuilder.DropIndex(
                name: "IX_ProductStocks_ConsignmentId",
                table: "ProductStocks");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "ProductVariants");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "ProductStocks",
                newName: "count");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStocks_ProductVariants_ProductVariantId",
                table: "ProductStocks",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
