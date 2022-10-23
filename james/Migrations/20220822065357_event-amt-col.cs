using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class eventamtcol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "amount",
                table: "events",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amount",
                table: "events");
        }
    }
}
