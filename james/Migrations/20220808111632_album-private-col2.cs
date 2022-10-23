using Microsoft.EntityFrameworkCore.Migrations;

namespace james.Migrations
{
    public partial class albumprivatecol2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "hiddenAlbums",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_hiddenAlbums_userId",
                table: "hiddenAlbums",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_hiddenAlbums_users_userId",
                table: "hiddenAlbums",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hiddenAlbums_users_userId",
                table: "hiddenAlbums");

            migrationBuilder.DropIndex(
                name: "IX_hiddenAlbums_userId",
                table: "hiddenAlbums");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "hiddenAlbums");
        }
    }
}
