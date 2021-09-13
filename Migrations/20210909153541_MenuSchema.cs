using Microsoft.EntityFrameworkCore.Migrations;

namespace NUNA.Migrations
{
    public partial class MenuSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlagAktif",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Menu");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Menu",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Menu",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Parent",
                table: "Menu",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "Parent",
                table: "Menu");

            migrationBuilder.AddColumn<bool>(
                name: "FlagAktif",
                table: "Menu",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Menu",
                type: "integer",
                nullable: true);
        }
    }
}
