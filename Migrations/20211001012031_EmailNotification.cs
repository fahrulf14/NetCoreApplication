using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace NUNA.Migrations
{
    public partial class EmailNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetSecretTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetSecretTokens", x => new { x.UserId, x.Token });
                    table.ForeignKey(
                        name: "FK_AspNetSecretTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuTopbar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    Nama = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ControllerName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ActionName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    IconClass = table.Column<string>(type: "text", nullable: true),
                    IsParent = table.Column<bool>(type: "boolean", nullable: false),
                    Parent = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    NoUrut = table.Column<int>(type: "integer", nullable: true),
                    Badge = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    BadgeStates = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuTopbar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MS_EmailSender",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sender = table.Column<string>(type: "text", nullable: true),
                    SmtpHost = table.Column<string>(type: "text", nullable: true),
                    SmtpPort = table.Column<string>(type: "text", nullable: true),
                    SmtpUser = table.Column<string>(type: "text", nullable: true),
                    SmtpCred = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreationUser = table.Column<string>(type: "text", nullable: true),
                    ModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModificationUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_EmailSender", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TR_EmailNotification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonalId = table.Column<int>(type: "integer", nullable: false),
                    NotifTypeId = table.Column<int>(type: "integer", nullable: false),
                    EmailDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SendDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EmailTo = table.Column<string>(type: "text", nullable: true),
                    CC = table.Column<string>(type: "text", nullable: true),
                    BCC = table.Column<string>(type: "text", nullable: true),
                    Subject = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    PersonalsId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TR_EmailNotification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TR_EmailNotification_Personals_PersonalsId",
                        column: x => x.PersonalsId,
                        principalTable: "Personals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MS_NotificationType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SenderId = table.Column<int>(type: "integer", nullable: false),
                    NotifTypeCode = table.Column<string>(type: "text", nullable: true),
                    NotifTypeName = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreationUser = table.Column<string>(type: "text", nullable: true),
                    ModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModificationUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_NotificationType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MS_NotificationType_MS_EmailSender_SenderId",
                        column: x => x.SenderId,
                        principalTable: "MS_EmailSender",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TR_EmailAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmailId = table.Column<int>(type: "integer", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    Attachment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TR_EmailAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TR_EmailAttachment_TR_EmailNotification_EmailId",
                        column: x => x.EmailId,
                        principalTable: "TR_EmailNotification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TR_EmailStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmailId = table.Column<int>(type: "integer", nullable: false),
                    ExecutionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TR_EmailStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TR_EmailStatus_TR_EmailNotification_EmailId",
                        column: x => x.EmailId,
                        principalTable: "TR_EmailNotification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetSecretTokens_UserId",
                table: "AspNetSecretTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MS_NotificationType_SenderId",
                table: "MS_NotificationType",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TR_EmailAttachment_EmailId",
                table: "TR_EmailAttachment",
                column: "EmailId");

            migrationBuilder.CreateIndex(
                name: "IX_TR_EmailNotification_PersonalsId",
                table: "TR_EmailNotification",
                column: "PersonalsId");

            migrationBuilder.CreateIndex(
                name: "IX_TR_EmailStatus_EmailId",
                table: "TR_EmailStatus",
                column: "EmailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetSecretTokens");

            migrationBuilder.DropTable(
                name: "MenuTopbar");

            migrationBuilder.DropTable(
                name: "MS_NotificationType");

            migrationBuilder.DropTable(
                name: "TR_EmailAttachment");

            migrationBuilder.DropTable(
                name: "TR_EmailStatus");

            migrationBuilder.DropTable(
                name: "MS_EmailSender");

            migrationBuilder.DropTable(
                name: "TR_EmailNotification");
        }
    }
}
