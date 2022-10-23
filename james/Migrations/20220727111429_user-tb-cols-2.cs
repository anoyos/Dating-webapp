using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class usertbcols2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "likeAndDisLikes");

            migrationBuilder.AlterColumn<int>(
                name: "whereamiknow",
                table: "users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "gelocationBydistance",
                table: "users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "matches",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fromUserId = table.Column<int>(type: "int", nullable: false),
                    toUserId = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_matches", x => x.id);
                    table.ForeignKey(
                        name: "FK_matches_users_fromUserId",
                        column: x => x.fromUserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_matches_users_toUserId",
                        column: x => x.toUserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_matches_fromUserId",
                table: "matches",
                column: "fromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_matches_toUserId",
                table: "matches",
                column: "toUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "matches");

            migrationBuilder.AlterColumn<int>(
                name: "whereamiknow",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "gelocationBydistance",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "likeAndDisLikes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fromUserId = table.Column<int>(type: "int", nullable: false),
                    isLike = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    toUserId = table.Column<int>(type: "int", nullable: false)
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
    }
}
