using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class notifflagcol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isNotificationFlag",
                table: "firebaseTokens",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isNotificationFlag",
                table: "firebaseTokens");
        }
    }
}
