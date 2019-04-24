using Microsoft.EntityFrameworkCore.Migrations;

namespace PeerIt.Migrations
{
    public partial class NamePropertyToPFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PFiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "PFiles");
        }
    }
}
