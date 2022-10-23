using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class vcodeuidcol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "uid",
                table: "verificationCodes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "uid",
                table: "verificationCodes");
        }
    }
}
