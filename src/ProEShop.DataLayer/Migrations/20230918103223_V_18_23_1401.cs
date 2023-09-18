using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProEShop.DataLayer.Migrations
{
    public partial class V_18_23_1401 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ParcelPosts_PostTrackingCode",
                table: "ParcelPosts");

            migrationBuilder.AlterColumn<string>(
                name: "PostTrackingCode",
                table: "ParcelPosts",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.CreateIndex(
                name: "IX_ParcelPosts_PostTrackingCode",
                table: "ParcelPosts",
                column: "PostTrackingCode",
                unique: true,
                filter: "[PostTrackingCode] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ParcelPosts_PostTrackingCode",
                table: "ParcelPosts");

            migrationBuilder.AlterColumn<string>(
                name: "PostTrackingCode",
                table: "ParcelPosts",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParcelPosts_PostTrackingCode",
                table: "ParcelPosts",
                column: "PostTrackingCode",
                unique: true);
        }
    }
}
