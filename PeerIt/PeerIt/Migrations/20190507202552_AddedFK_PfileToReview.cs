using Microsoft.EntityFrameworkCore.Migrations;

namespace PeerIt.Migrations
{
    public partial class AddedFK_PfileToReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PFiles_StudentAssignments_StudentAssignmentID",
                table: "PFiles");

            migrationBuilder.DropIndex(
                name: "IX_PFiles_StudentAssignmentID",
                table: "PFiles");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "StudentAssignmentID",
                table: "PFiles");

            migrationBuilder.AddColumn<string>(
                name: "FK_PFileID",
                table: "Reviews",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_FK_PFileID",
                table: "Reviews",
                column: "FK_PFileID");

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

            migrationBuilder.DropIndex(
                name: "IX_Reviews_FK_PFileID",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "FK_PFileID",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Reviews",
                maxLength: 100000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StudentAssignmentID",
                table: "PFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PFiles_StudentAssignmentID",
                table: "PFiles",
                column: "StudentAssignmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_PFiles_StudentAssignments_StudentAssignmentID",
                table: "PFiles",
                column: "StudentAssignmentID",
                principalTable: "StudentAssignments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
