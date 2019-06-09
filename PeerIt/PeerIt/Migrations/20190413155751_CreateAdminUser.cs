using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;
using PeerIt.Models;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PeerIt.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

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
            
            DateTime dt = DateTime.Now;

            string dateString =  dt.ToString("yyyy-MM-dd HH:mm:ss");
            AppUser newAdmin = new AppUser()
            {
                Id = AdminUserId,
                UserName = "admin",
                NormalizedUserName = "admin@example.com",
                Email = "admin@example.com",
                NormalizedEmail = "admin@example.com",
                EmailConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                TimestampCreated = dt,
                FirstName = "Admin",
                LastName = "Admin",
                IsEnabled = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var password = new PasswordHasher<AppUser>();
            newAdmin.PasswordHash = password.HashPassword(newAdmin, "password"); ;

            migrationBuilder.InsertData(
            table: "AspNetUsers",
                columns: new[] {
                    "Id","UserName","Email","EmailConfirmed","PasswordHash",
                    "TwoFactorEnabled","LockoutEnabled","AccessFailedcount","TimestampCreated","FirstName","LastName","IsEnabled","PhoneNumberConfirmed","SecurityStamp","NormalizedEmail","NormalizedUserName"},
                values: new object[] {
                   newAdmin.Id.ToString(),newAdmin.UserName.ToString(),newAdmin.Email.ToString(),newAdmin.EmailConfirmed,newAdmin.PasswordHash,newAdmin.TwoFactorEnabled,
                   newAdmin.LockoutEnabled,newAdmin.AccessFailedCount.ToString(),newAdmin.TimestampCreated.ToString("yyyy-MM-dd HH:mm:ss"),newAdmin.FirstName,
                   newAdmin.LastName.ToString(),newAdmin.IsEnabled,newAdmin.PhoneNumberConfirmed,newAdmin.SecurityStamp,newAdmin.NormalizedEmail,newAdmin.NormalizedUserName
                }   
            );

            //Add Admin into Admin Role
            migrationBuilder.InsertData(
                 table: "AspNetUserRoles",
                 columns: new[] { "UserId", "RoleId" },
                 values: new object[] { AdminUserId, AdminRoleId }
             );
            // Add Settings
            migrationBuilder.InsertData(
                table: "Settings",
                 columns: new[] { "ID", "StringValue", "NumericValue" },
                 values: new object[] { "SMTP_HOST", "localhost", 0}
                );
            migrationBuilder.InsertData(
                table: "Settings",
                 columns: new[] { "ID", "NumericValue" },
                 values: new object[] { "SMTP_Port", 25 }
                );
            migrationBuilder.InsertData(
                table: "Settings",
                 columns: new[] { "ID", "StringValue", "NumericValue" },
                 values: new object[] { "SMTP_Password", "something", 0 }
                );
            migrationBuilder.InsertData(
               table: "Settings",
                columns: new[] { "ID", "NumericValue" },
                values: new object[] { "SMTP_Enabled", 0 }
               );
            migrationBuilder.InsertData(
               table: "Settings",
                columns: new[] { "ID", "StringValue", "Numericvalue" },
                values: new object[] { "SMTP_USERNAME", "root", 0 }
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

