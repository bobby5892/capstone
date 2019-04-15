using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;
using PeerIt.Models;
using System;

namespace PeerIt.Migrations
{
    public partial class CreateAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /* Initial Seed */
            string AdminRoleId = "8820ee66-681f-45b3-92ed-7d8dee5d4beg";
            string StudentRoleId = "8820ee66-681f-45b3-92ed-7d8dee5d4beh";
            string InstructorRoleId = "8820ee66-681f-45b3-92ed-7d8dee5d4bei";
            string InvitedGuestRoleId = "8820ee66-681f-45b3-92ed-7d8dee5d4bej";

            Console.WriteLine("Creating Admin Role");
            /* Add Roles */
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName",},
                values: new object[] { AdminRoleId.ToString(), "Administrator", "Administrator" }
            );
            Console.WriteLine("Creating Student Role");
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName"},
                values: new object[] { StudentRoleId.ToString(), "Student", "Student" }
            );
            Console.WriteLine("Creating Instructor Role");
            migrationBuilder.InsertData(
                 table: "AspNetRoles",
                 columns: new[] { "Id", "Name", "NormalizedName" },
                 values: new object[] { InstructorRoleId.ToString(), "Instructor", "Instructor" }
             );
            Console.WriteLine("Creating InvitedGuest Role");
            migrationBuilder.InsertData(
                 table: "AspNetRoles",
                 columns: new[] { "Id", "Name", "NormalizedName" },
                 values: new object[] { InvitedGuestRoleId.ToString(), "InvitedGuest", "InvitedGuest" }
             );
            string AdminUserId = "8820ee66-681f-45b3-92ed-7d8dee5d4beb";
            string AdminPassHash = "ZWFyZ0ZSZXJnaWpkZm9haWozNGczNA==";
            string SecurityStamp = "AQAAAAEAACcQAAAAECOsH8Nrr4kIt+J+quzqMSXx6fYN55IHg2QPdBFpa+diUvw5lwNpBD/1SvBkQMfZnA==";

            // Add Admin User
            // Note - Security stamp (although the model says nullable - is not nullable)
            // it requires a Base64 random string
            DateTime dt = new DateTime(2019, 4, 13, 9, 56, 2);
            
            migrationBuilder.InsertData(
            table: "AspNetUsers",
                columns: new[] {
                    "Id","UserName","NormalizedUserName","Email","NormalizedEmail","EmailConfirmed","PasswordHash","SecurityStamp","PhoneNumberConfirmed",
                    "TwoFactorEnabled","LockoutEnabled","AccessFailedcount","TimestampCreated","FirstName","LastName","IsEnabled"},
                values: new object[] {
                    AdminUserId,"admin","admin","admin@example.com","admin@example.com",true,
                    SecurityStamp,AdminPassHash,true,false,false,0
                    ,dt,"Admin","Admin",true}
            );
            //Add Admin into Admin Role
            migrationBuilder.InsertData(
                 table: "AspNetUserRoles",
                 columns: new[] { "UserId", "RoleId" },
                 values: new object[] { AdminUserId, AdminRoleId }
             );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM AspNetRoles", true);
            migrationBuilder.Sql("DELETE FROM AspNetUsers", true);
            migrationBuilder.Sql("DELETE FROM AspNetUserRoles", true);
            
        }
    }
}

