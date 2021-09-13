using Microsoft.EntityFrameworkCore.Migrations;

namespace SIP.Migrations
{
    public partial class AddMenuPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoleMenus",
                columns: table => new
                {
                    RoleId = table.Column<string>(maxLength: 36, nullable: false),
                    Menu = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleMenus", x => new { x.RoleId, x.Menu });
                    table.ForeignKey(
                        name: "FK_AspNetRoleMenus_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserMenus",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 36, nullable: false),
                    Menu = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserMenus", x => new { x.UserId, x.Menu });
                    table.ForeignKey(
                        name: "FK_AspNetUserMenus_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleMenus_RoleId",
                table: "AspNetRoleMenus",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserMenus_UserId",
                table: "AspNetUserMenus",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleMenus");

            migrationBuilder.DropTable(
                name: "AspNetUserMenus");
        }
    }
}
