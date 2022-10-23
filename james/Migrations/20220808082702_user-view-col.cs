using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class userviewcol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "views",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "views",
                table: "users");
        }
    }
}
