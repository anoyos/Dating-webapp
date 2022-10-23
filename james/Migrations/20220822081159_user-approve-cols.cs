using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class userapprovecols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isApprove",
                table: "users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isBlocked",
                table: "users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isFlagged",
                table: "users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "userAutoApprove",
                table: "appSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isApprove",
                table: "users");

            migrationBuilder.DropColumn(
                name: "isBlocked",
                table: "users");

            migrationBuilder.DropColumn(
                name: "isFlagged",
                table: "users");

            migrationBuilder.DropColumn(
                name: "userAutoApprove",
                table: "appSettings");
        }
    }
}
