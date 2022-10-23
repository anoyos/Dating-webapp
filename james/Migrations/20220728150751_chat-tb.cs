using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class chattb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "chats",
                newName: "messageType");

            migrationBuilder.AddColumn<string>(
                name: "attachment",
                table: "chats",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "duration",
                table: "chats",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "chatThreads",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user1Id = table.Column<int>(type: "int", nullable: false),
                    user2Id = table.Column<int>(type: "int", nullable: false),
                    chatId = table.Column<int>(type: "int", nullable: false),
                    last_message = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    last_message_timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    user1_unread = table.Column<int>(type: "int", nullable: false),
                    user2_unread = table.Column<int>(type: "int", nullable: false),
                    chat_id = table.Column<int>(type: "int", nullable: true),
                    message_type = table.Column<int>(type: "int", nullable: true),
                    attachment = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    additional_info = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatThreads", x => x.id);
                    table.ForeignKey(
                        name: "FK_chatThreads_chats_chatId",
                        column: x => x.chatId,
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_chatThreads_users_user1Id",
                        column: x => x.user1Id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_chatThreads_users_user2Id",
                        column: x => x.user2Id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "firebaseTokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    token = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    device = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    os = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_firebaseTokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_firebaseTokens_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_chatThreads_chatId",
                table: "chatThreads",
                column: "chatId");

            migrationBuilder.CreateIndex(
                name: "IX_chatThreads_user1Id",
                table: "chatThreads",
                column: "user1Id");

            migrationBuilder.CreateIndex(
                name: "IX_chatThreads_user2Id",
                table: "chatThreads",
                column: "user2Id");

            migrationBuilder.CreateIndex(
                name: "IX_firebaseTokens_userId",
                table: "firebaseTokens",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chatThreads");

            migrationBuilder.DropTable(
                name: "firebaseTokens");

            migrationBuilder.DropColumn(
                name: "attachment",
                table: "chats");

            migrationBuilder.DropColumn(
                name: "duration",
                table: "chats");

            migrationBuilder.RenameColumn(
                name: "messageType",
                table: "chats",
                newName: "type");
        }
    }
}
