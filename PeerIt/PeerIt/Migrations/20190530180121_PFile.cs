using Microsoft.EntityFrameworkCore.Migrations;

namespace PeerIt.Migrations
{
    public partial class PFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "StudentAssignments");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "StudentAssignments",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "FK_PFileID",
                table: "StudentAssignments",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAssignments_FK_PFileID",
                table: "StudentAssignments",
                column: "FK_PFileID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAssignments_PFiles_FK_PFileID",
                table: "StudentAssignments",
                column: "FK_PFileID",
                principalTable: "PFiles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAssignments_PFiles_FK_PFileID",
                table: "StudentAssignments");

            migrationBuilder.DropIndex(
                name: "IX_StudentAssignments_FK_PFileID",
                table: "StudentAssignments");

            migrationBuilder.DropColumn(
                name: "FK_PFileID",
                table: "StudentAssignments");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "StudentAssignments",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "StudentAssignments",
                nullable: false,
                defaultValue: 0);
        }
    }
}
