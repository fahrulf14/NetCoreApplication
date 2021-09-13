using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SIP.Migrations
{
    public partial class AddPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserSetting",
                maxLength: 75,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Personal",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NIP",
                table: "Personal",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Personal",
                maxLength: 75,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalCode",
                table: "Personal",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RF_PositionId",
                table: "Personal",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Parent",
                table: "Menu",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nama",
                table: "Menu",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IconClass",
                table: "Menu",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(75)",
                oldMaxLength: 75,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Controller",
                table: "Menu",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Menu",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ActionName",
                table: "Menu",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AspNetRolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(maxLength: 36, nullable: false),
                    Permission = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRolePermissions", x => new { x.RoleId, x.Permission });
                    table.ForeignKey(
                        name: "FK_AspNetRolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserPermissions",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 36, nullable: false),
                    Permission = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserPermissions", x => new { x.UserId, x.Permission });
                    table.ForeignKey(
                        name: "FK_AspNetUserPermissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personal_RF_PositionId",
                table: "Personal",
                column: "RF_PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRolePermissions_RoleId",
                table: "AspNetRolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserPermissions_UserId",
                table: "AspNetUserPermissions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personal_RF_Position_RF_PositionId",
                table: "Personal",
                column: "RF_PositionId",
                principalTable: "RF_Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personal_RF_Position_RF_PositionId",
                table: "Personal");

            migrationBuilder.DropTable(
                name: "AspNetRolePermissions");

            migrationBuilder.DropTable(
                name: "AspNetUserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_Personal_RF_PositionId",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "PersonalCode",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "RF_PositionId",
                table: "Personal");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserSetting",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 75,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Personal",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NIP",
                table: "Personal",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Personal",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 75,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Parent",
                table: "Menu",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nama",
                table: "Menu",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IconClass",
                table: "Menu",
                type: "character varying(75)",
                maxLength: 75,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Controller",
                table: "Menu",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Menu",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ActionName",
                table: "Menu",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);
        }
    }
}
