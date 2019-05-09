using Microsoft.EntityFrameworkCore.Migrations;

namespace PeerIt.Migrations
{
    public partial class AddedstudentassignmenttopFilemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentAssignmentID",
                table: "PFiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewGroup",
                table: "CourseGroups",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PFiles_StudentAssignments_StudentAssignmentID",
                table: "PFiles");

            migrationBuilder.DropIndex(
                name: "IX_PFiles_StudentAssignmentID",
                table: "PFiles");

            migrationBuilder.DropColumn(
                name: "StudentAssignmentID",
                table: "PFiles");

            migrationBuilder.DropColumn(
                name: "ReviewGroup",
                table: "CourseGroups");
        }
    }
}
