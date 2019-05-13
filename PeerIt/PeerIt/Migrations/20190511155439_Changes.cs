using Microsoft.EntityFrameworkCore.Migrations;

namespace PeerIt.Migrations
{
    public partial class Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "FK_PFileID",
                table: "Reviews",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReviewGroup",
                table: "CourseGroups",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PFiles",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Ext = table.Column<string>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PFiles_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_FK_PFileID",
                table: "Reviews",
                column: "FK_PFileID");

            migrationBuilder.CreateIndex(
                name: "IX_PFiles_AppUserId",
                table: "PFiles",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_PFiles_FK_PFileID",
                table: "Reviews",
                column: "FK_PFileID",
                principalTable: "PFiles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_PFiles_FK_PFileID",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "PFiles");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_FK_PFileID",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "FK_PFileID",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReviewGroup",
                table: "CourseGroups");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Reviews",
                maxLength: 100000,
                nullable: false,
                defaultValue: "");
        }
    }
}
