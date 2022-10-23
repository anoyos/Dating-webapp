using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class usertbcols4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "genderId",
                table: "users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "albums",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albums", x => x.id);
                    table.ForeignKey(
                        name: "FK_albums_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "chats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    type = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fromUserId = table.Column<int>(type: "int", nullable: false),
                    toUserId = table.Column<int>(type: "int", nullable: false),
                    isSeen = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chats", x => x.id);
                    table.ForeignKey(
                        name: "FK_chats_users_fromUserId",
                        column: x => x.fromUserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_chats_users_toUserId",
                        column: x => x.toUserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "diaries",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    dateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    photo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diaries", x => x.id);
                    table.ForeignKey(
                        name: "FK_diaries_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "reportUsers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reason = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    fromUserId = table.Column<int>(type: "int", nullable: false),
                    toUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reportUsers", x => x.id);
                    table.ForeignKey(
                        name: "FK_reportUsers_users_fromUserId",
                        column: x => x.fromUserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reportUsers_users_toUserId",
                        column: x => x.toUserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userRatings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    review = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    rating = table.Column<double>(type: "double", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fromUserId = table.Column<int>(type: "int", nullable: false),
                    toUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userRatings", x => x.id);
                    table.ForeignKey(
                        name: "FK_userRatings_users_fromUserId",
                        column: x => x.fromUserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_userRatings_users_toUserId",
                        column: x => x.toUserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "verificationCodes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_verificationCodes", x => x.id);
                    table.ForeignKey(
                        name: "FK_verificationCodes_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "albumImages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    photo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    albumId = table.Column<int>(type: "int", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albumImages", x => x.id);
                    table.ForeignKey(
                        name: "FK_albumImages_albums_albumId",
                        column: x => x.albumId,
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "hiddenAlbums",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<int>(type: "int", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    albumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hiddenAlbums", x => x.id);
                    table.ForeignKey(
                        name: "FK_hiddenAlbums_albums_albumId",
                        column: x => x.albumId,
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_users_genderId",
                table: "users",
                column: "genderId");

            migrationBuilder.CreateIndex(
                name: "IX_albumImages_albumId",
                table: "albumImages",
                column: "albumId");

            migrationBuilder.CreateIndex(
                name: "IX_albums_userId",
                table: "albums",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_chats_fromUserId",
                table: "chats",
                column: "fromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_chats_toUserId",
                table: "chats",
                column: "toUserId");

            migrationBuilder.CreateIndex(
                name: "IX_diaries_userId",
                table: "diaries",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_hiddenAlbums_albumId",
                table: "hiddenAlbums",
                column: "albumId");

            migrationBuilder.CreateIndex(
                name: "IX_reportUsers_fromUserId",
                table: "reportUsers",
                column: "fromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_reportUsers_toUserId",
                table: "reportUsers",
                column: "toUserId");

            migrationBuilder.CreateIndex(
                name: "IX_userRatings_fromUserId",
                table: "userRatings",
                column: "fromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_userRatings_toUserId",
                table: "userRatings",
                column: "toUserId");

            migrationBuilder.CreateIndex(
                name: "IX_verificationCodes_userId",
                table: "verificationCodes",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_ddls_genderId",
                table: "users",
                column: "genderId",
                principalTable: "ddls",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_ddls_genderId",
                table: "users");

            migrationBuilder.DropTable(
                name: "albumImages");

            migrationBuilder.DropTable(
                name: "chats");

            migrationBuilder.DropTable(
                name: "diaries");

            migrationBuilder.DropTable(
                name: "hiddenAlbums");

            migrationBuilder.DropTable(
                name: "reportUsers");

            migrationBuilder.DropTable(
                name: "userRatings");

            migrationBuilder.DropTable(
                name: "verificationCodes");

            migrationBuilder.DropTable(
                name: "albums");

            migrationBuilder.DropIndex(
                name: "IX_users_genderId",
                table: "users");

            migrationBuilder.AlterColumn<int>(
                name: "genderId",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
