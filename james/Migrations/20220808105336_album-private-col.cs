using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class albumprivatecol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPrivate",
                table: "albums",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPrivate",
                table: "albums");
        }
    }
}
