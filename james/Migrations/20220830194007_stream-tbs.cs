using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class streamtbs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "streamings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    banner = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    category = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    token = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    channel = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    startedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    endedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streamings", x => x.id);
                    table.ForeignKey(
                        name: "FK_streamings_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "streamingActivities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    type = table.Column<int>(type: "int", nullable: false),
                    detail = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    streamId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    parentId = table.Column<int>(type: "int", nullable: true),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streamingActivities", x => x.id);
                    table.ForeignKey(
                        name: "FK_streamingActivities_streamingActivities_parentId",
                        column: x => x.parentId,
                        principalTable: "streamingActivities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_streamingActivities_streamings_streamId",
                        column: x => x.streamId,
                        principalTable: "streamings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_streamingActivities_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "streamingMembers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    streamId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streamingMembers", x => x.id);
                    table.ForeignKey(
                        name: "FK_streamingMembers_streamings_streamId",
                        column: x => x.streamId,
                        principalTable: "streamings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_streamingMembers_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_streamingActivities_parentId",
                table: "streamingActivities",
                column: "parentId");

            migrationBuilder.CreateIndex(
                name: "IX_streamingActivities_streamId",
                table: "streamingActivities",
                column: "streamId");

            migrationBuilder.CreateIndex(
                name: "IX_streamingActivities_userId",
                table: "streamingActivities",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_streamingMembers_streamId",
                table: "streamingMembers",
                column: "streamId");

            migrationBuilder.CreateIndex(
                name: "IX_streamingMembers_userId",
                table: "streamingMembers",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_streamings_userId",
                table: "streamings",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "streamingActivities");

            migrationBuilder.DropTable(
                name: "streamingMembers");

            migrationBuilder.DropTable(
                name: "streamings");
        }
    }
}
