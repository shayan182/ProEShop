using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProEShop.DataLayer.Migrations
{
    public partial class V2022_11_15_1846 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MonthCount",
                table: "Guarantees",
                newName: "MonthsCount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MonthsCount",
                table: "Guarantees",
                newName: "MonthCount");
        }
    }
}
