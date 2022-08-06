using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProEShop.DataLayer.Migrations
{
    public partial class V2022_08_03_1540 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDocumentApproved",
                table: "Sellers");

            migrationBuilder.AddColumn<byte>(
                name: "DocumentStatus",
                table: "Sellers",
                type: "tinyint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentStatus",
                table: "Sellers");

            migrationBuilder.AddColumn<bool>(
                name: "IsDocumentApproved",
                table: "Sellers",
                type: "bit",
                nullable: true);
        }
    }
}
