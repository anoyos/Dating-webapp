using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class likedisliketb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "lastLocTimestamp",
                table: "users",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lat",
                table: "users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "likes",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "lng",
                table: "users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "rating",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "source",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "token",
                table: "users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "likeAndDisLikes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fromUserId = table.Column<int>(type: "int", nullable: false),
                    toUserId = table.Column<int>(type: "int", nullable: false),
                    isLike = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_likeAndDisLikes", x => x.id);
                    table.ForeignKey(
                        name: "FK_likeAndDisLikes_users_fromUserId",
                        column: x => x.fromUserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_likeAndDisLikes_users_toUserId",
                        column: x => x.toUserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_likeAndDisLikes_fromUserId",
                table: "likeAndDisLikes",
                column: "fromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_likeAndDisLikes_toUserId",
                table: "likeAndDisLikes",
                column: "toUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "likeAndDisLikes");

            migrationBuilder.DropColumn(
                name: "email",
                table: "users");

            migrationBuilder.DropColumn(
                name: "lastLocTimestamp",
                table: "users");

            migrationBuilder.DropColumn(
                name: "lat",
                table: "users");

            migrationBuilder.DropColumn(
                name: "likes",
                table: "users");

            migrationBuilder.DropColumn(
                name: "lng",
                table: "users");

            migrationBuilder.DropColumn(
                name: "rating",
                table: "users");

            migrationBuilder.DropColumn(
                name: "source",
                table: "users");

            migrationBuilder.DropColumn(
                name: "token",
                table: "users");
        }
    }
}
