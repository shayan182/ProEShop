using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProEShop.DataLayer.Migrations
{
    public partial class V2022_07_20_1536 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProvinceAndCities_ProvinceAndCities_ParentId",
                table: "ProvinceAndCities");

            migrationBuilder.DropForeignKey(
                name: "FK_Sellers_ProvinceAndCities_CityId",
                table: "Sellers");

            migrationBuilder.DropForeignKey(
                name: "FK_Sellers_ProvinceAndCities_ProvinceId",
                table: "Sellers");

            migrationBuilder.DropForeignKey(
                name: "FK_Sellers_Users_UserId",
                table: "Sellers");

            migrationBuilder.DropIndex(
                name: "IX_Sellers_ShopName",
                table: "Sellers");

            migrationBuilder.DropIndex(
                name: "IX_Sellers_UserId",
                table: "Sellers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProvinceAndCities",
                table: "ProvinceAndCities");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Sellers");

            migrationBuilder.RenameTable(
                name: "ProvinceAndCities",
                newName: "ProvincesAndCities");

            migrationBuilder.RenameIndex(
                name: "IX_ProvinceAndCities_ParentId",
                table: "ProvincesAndCities",
                newName: "IX_ProvincesAndCities_ParentId");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Sellers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Sellers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShopName",
                table: "Sellers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "Sellers",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDocumentApproved",
                table: "Sellers",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Sellers",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "IdCartPicture",
                table: "Sellers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Sellers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelePhone",
                table: "Sellers",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProvincesAndCities",
                table: "ProvincesAndCities",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_SellerCode",
                table: "Sellers",
                column: "SellerCode",
                unique: true,
                filter: "[SellerCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_ShopName",
                table: "Sellers",
                column: "ShopName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_UserId",
                table: "Sellers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProvincesAndCities_ProvincesAndCities_ParentId",
                table: "ProvincesAndCities",
                column: "ParentId",
                principalTable: "ProvincesAndCities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sellers_ProvincesAndCities_CityId",
                table: "Sellers",
                column: "CityId",
                principalTable: "ProvincesAndCities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sellers_ProvincesAndCities_ProvinceId",
                table: "Sellers",
                column: "ProvinceId",
                principalTable: "ProvincesAndCities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sellers_Users_UserId",
                table: "Sellers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProvincesAndCities_ProvincesAndCities_ParentId",
                table: "ProvincesAndCities");

            migrationBuilder.DropForeignKey(
                name: "FK_Sellers_ProvincesAndCities_CityId",
                table: "Sellers");

            migrationBuilder.DropForeignKey(
                name: "FK_Sellers_ProvincesAndCities_ProvinceId",
                table: "Sellers");

            migrationBuilder.DropForeignKey(
                name: "FK_Sellers_Users_UserId",
                table: "Sellers");

            migrationBuilder.DropIndex(
                name: "IX_Sellers_SellerCode",
                table: "Sellers");

            migrationBuilder.DropIndex(
                name: "IX_Sellers_ShopName",
                table: "Sellers");

            migrationBuilder.DropIndex(
                name: "IX_Sellers_UserId",
                table: "Sellers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProvincesAndCities",
                table: "ProvincesAndCities");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TelePhone",
                table: "Sellers");

            migrationBuilder.RenameTable(
                name: "ProvincesAndCities",
                newName: "ProvinceAndCities");

            migrationBuilder.RenameIndex(
                name: "IX_ProvincesAndCities_ParentId",
                table: "ProvinceAndCities",
                newName: "IX_ProvinceAndCities_ParentId");

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Sellers",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "ShopName",
                table: "Sellers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "Sellers",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDocumentApproved",
                table: "Sellers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Sellers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdCartPicture",
                table: "Sellers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Sellers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProvinceAndCities",
                table: "ProvinceAndCities",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_ShopName",
                table: "Sellers",
                column: "ShopName",
                unique: true,
                filter: "[ShopName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_UserId",
                table: "Sellers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProvinceAndCities_ProvinceAndCities_ParentId",
                table: "ProvinceAndCities",
                column: "ParentId",
                principalTable: "ProvinceAndCities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sellers_ProvinceAndCities_CityId",
                table: "Sellers",
                column: "CityId",
                principalTable: "ProvinceAndCities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sellers_ProvinceAndCities_ProvinceId",
                table: "Sellers",
                column: "ProvinceId",
                principalTable: "ProvinceAndCities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sellers_Users_UserId",
                table: "Sellers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
