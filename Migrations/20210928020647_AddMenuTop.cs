using Microsoft.EntityFrameworkCore.Migrations;

namespace NUNA.Migrations
{
    public partial class AddMenuTop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_NormalizedUserName");

            migrationBuilder.RenameIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_NormalizedEmail");

            migrationBuilder.RenameIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                newName: "IX_AspNetRoles_NormalizedName");

            migrationBuilder.AddColumn<string>(
                name: "Badge",
                table: "Menu",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BadgeStates",
                table: "Menu",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Badge",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "BadgeStates",
                table: "Menu");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_NormalizedUserName",
                table: "AspNetUsers",
                newName: "UserNameIndex");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_NormalizedEmail",
                table: "AspNetUsers",
                newName: "EmailIndex");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoles_NormalizedName",
                table: "AspNetRoles",
                newName: "RoleNameIndex");
        }
    }
}
