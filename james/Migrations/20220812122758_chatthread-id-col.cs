using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class chatthreadidcol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chats_users_fromUserId",
                table: "chats");

            migrationBuilder.DropForeignKey(
                name: "FK_chats_users_toUserId",
                table: "chats");

            migrationBuilder.DropColumn(
                name: "chat_id",
                table: "chatThreads");

            migrationBuilder.RenameColumn(
                name: "toUserId",
                table: "chats",
                newName: "senderId");

            migrationBuilder.RenameColumn(
                name: "fromUserId",
                table: "chats",
                newName: "chatThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_chats_toUserId",
                table: "chats",
                newName: "IX_chats_senderId");

            migrationBuilder.RenameIndex(
                name: "IX_chats_fromUserId",
                table: "chats",
                newName: "IX_chats_chatThreadId");

            migrationBuilder.AlterColumn<int>(
                name: "message_type",
                table: "chatThreads",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "chatId",
                table: "chatThreads",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_chats_chatThreads_chatThreadId",
                table: "chats",
                column: "chatThreadId",
                principalTable: "chatThreads",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_chats_users_senderId",
                table: "chats",
                column: "senderId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chats_chatThreads_chatThreadId",
                table: "chats");

            migrationBuilder.DropForeignKey(
                name: "FK_chats_users_senderId",
                table: "chats");

            migrationBuilder.RenameColumn(
                name: "senderId",
                table: "chats",
                newName: "toUserId");

            migrationBuilder.RenameColumn(
                name: "chatThreadId",
                table: "chats",
                newName: "fromUserId");

            migrationBuilder.RenameIndex(
                name: "IX_chats_senderId",
                table: "chats",
                newName: "IX_chats_toUserId");

            migrationBuilder.RenameIndex(
                name: "IX_chats_chatThreadId",
                table: "chats",
                newName: "IX_chats_fromUserId");

            migrationBuilder.AlterColumn<int>(
                name: "message_type",
                table: "chatThreads",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "chatId",
                table: "chatThreads",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "chat_id",
                table: "chatThreads",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_chats_users_fromUserId",
                table: "chats",
                column: "fromUserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_chats_users_toUserId",
                table: "chats",
                column: "toUserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
