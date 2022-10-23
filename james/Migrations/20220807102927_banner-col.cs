using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class bannercol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "banner",
                table: "users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "banner",
                table: "users");
        }
    }
}
