using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class usertbgendercol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "rating",
                table: "users",
                type: "double",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "city",
                table: "users");

            migrationBuilder.AlterColumn<int>(
                name: "rating",
                table: "users",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");
        }
    }
}
