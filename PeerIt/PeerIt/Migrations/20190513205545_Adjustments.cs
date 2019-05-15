using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeerIt.Migrations
{
    public partial class Adjustments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "InstructionText",
                table: "CourseAssignments");

            migrationBuilder.DropColumn(
                name: "InstructionsUrl",
                table: "CourseAssignments");

            migrationBuilder.DropColumn(
                name: "RubricText",
                table: "CourseAssignments");

            migrationBuilder.DropColumn(
                name: "RubricUrl",
                table: "CourseAssignments");

            migrationBuilder.AddColumn<string>(
                name: "FK_PFileID",
                table: "Reviews",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReviewGroup",
                table: "CourseGroups",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "CourseAssignments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PFileID",
                table: "CourseAssignments",
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
                name: "IX_CourseAssignments_PFileID",
                table: "CourseAssignments",
                column: "PFileID");

            migrationBuilder.CreateIndex(
                name: "IX_PFiles_AppUserId",
                table: "PFiles",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignments_PFiles_PFileID",
                table: "CourseAssignments",
                column: "PFileID",
                principalTable: "PFiles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_CourseAssignments_PFiles_PFileID",
                table: "CourseAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_PFiles_FK_PFileID",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "PFiles");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_FK_PFileID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_CourseAssignments_PFileID",
                table: "CourseAssignments");

            migrationBuilder.DropColumn(
                name: "FK_PFileID",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReviewGroup",
                table: "CourseGroups");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "CourseAssignments");

            migrationBuilder.DropColumn(
                name: "PFileID",
                table: "CourseAssignments");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Reviews",
                maxLength: 100000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstructionText",
                table: "CourseAssignments",
                maxLength: 100000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructionsUrl",
                table: "CourseAssignments",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RubricText",
                table: "CourseAssignments",
                maxLength: 100000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RubricUrl",
                table: "CourseAssignments",
                maxLength: 200,
                nullable: true);
        }
    }
}
