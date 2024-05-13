using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class updatevarchar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetDetails_AssetCategories_CategoryId",
                table: "AssetDetails");

            migrationBuilder.DropIndex(
                name: "IX_AssetDetails_CategoryId",
                table: "AssetDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Users",
                type: "varchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "assetCategoryId",
                table: "AssetDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetDetails_assetCategoryId",
                table: "AssetDetails",
                column: "assetCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetDetails_AssetCategories_assetCategoryId",
                table: "AssetDetails",
                column: "assetCategoryId",
                principalTable: "AssetCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetDetails_AssetCategories_assetCategoryId",
                table: "AssetDetails");

            migrationBuilder.DropIndex(
                name: "IX_AssetDetails_assetCategoryId",
                table: "AssetDetails");

            migrationBuilder.DropColumn(
                name: "assetCategoryId",
                table: "AssetDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Users",
                type: "varchar(4)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetDetails_CategoryId",
                table: "AssetDetails",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetDetails_AssetCategories_CategoryId",
                table: "AssetDetails",
                column: "CategoryId",
                principalTable: "AssetCategories",
                principalColumn: "Id");
        }
    }
}
