using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class updatetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Users_UserId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "AssetName",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Asset Name",
                table: "AssetCategories",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Users",
                type: "varchar(4)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Users",
                type: "varchar(12)",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Assets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AssetId",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Assets",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialId",
                table: "AssetDetails",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "serialId",
                table: "AssetCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetId",
                table: "Assets",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_AssetDetails_AssetId",
                table: "Assets",
                column: "AssetId",
                principalTable: "AssetDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Users_UserId",
                table: "Assets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AssetDetails_AssetId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Users_UserId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_AssetId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "SerialId",
                table: "AssetDetails");

            migrationBuilder.DropColumn(
                name: "serialId",
                table: "AssetCategories");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AssetCategories",
                newName: "Asset Name");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Assets",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "AssetId",
                table: "Assets",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AssetName",
                table: "Assets",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Users_UserId",
                table: "Assets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
