﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PeerIt.Repositories;

namespace PeerIt.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20190424161835_NamePropertyToPFile")]
    partial class NamePropertyToPFile
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PeerIt.Models.ActiveReviewer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FK_APP_USER_VIEWERId")
                        .IsRequired();

                    b.Property<int>("FK_STUDENT_ASSIGNMENTID");

                    b.HasKey("ID");

                    b.HasIndex("FK_APP_USER_VIEWERId");

                    b.HasIndex("FK_STUDENT_ASSIGNMENTID");

                    b.ToTable("ActiveReviewers");
                });

            modelBuilder.Entity("PeerIt.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("PictureFilename")
                        .HasMaxLength(20);

                    b.Property<string>("SecurityStamp");

                    b.Property<DateTime>("TimestampCreated");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("PeerIt.Models.Comment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("FK_APP_USERId")
                        .IsRequired();

                    b.Property<int>("FK_STUDENT_ASSIGNMENTID");

                    b.Property<DateTime>("TimestampCreated");

                    b.HasKey("ID");

                    b.HasIndex("FK_APP_USERId");

                    b.HasIndex("FK_STUDENT_ASSIGNMENTID");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("PeerIt.Models.Course", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FK_INSTRUCTORId")
                        .IsRequired();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.HasKey("ID");

                    b.HasIndex("FK_INSTRUCTORId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("PeerIt.Models.CourseAssignment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FK_COURSEID");

                    b.Property<string>("InstructionText")
                        .HasMaxLength(100000);

                    b.Property<string>("InstructionsUrl")
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("RubricText")
                        .HasMaxLength(100000);

                    b.Property<string>("RubricUrl")
                        .HasMaxLength(200);

                    b.HasKey("ID");

                    b.HasIndex("FK_COURSEID");

                    b.ToTable("CourseAssignments");
                });

            modelBuilder.Entity("PeerIt.Models.CourseGroup", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FK_AppUserId")
                        .IsRequired();

                    b.Property<int>("FK_CourseID");

                    b.HasKey("ID");

                    b.HasIndex("FK_AppUserId");

                    b.HasIndex("FK_CourseID");

                    b.ToTable("CourseGroups");
                });

            modelBuilder.Entity("PeerIt.Models.Event", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(5000);

                    b.Property<string>("FK_AppUserId")
                        .IsRequired();

                    b.Property<bool>("HasSeen");

                    b.Property<DateTime>("TimeStampCreated");

                    b.HasKey("ID");

                    b.HasIndex("FK_AppUserId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("PeerIt.Models.ForgotPassword", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FK_APP_USERId")
                        .IsRequired();

                    b.Property<string>("ResetHash")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("FK_APP_USERId");

                    b.ToTable("ForgotPasswords");
                });

            modelBuilder.Entity("PeerIt.Models.Invitation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FK_APP_USER_RECIPIENTId")
                        .IsRequired();

                    b.Property<string>("FK_APP_USER_SENDERId")
                        .IsRequired();

                    b.Property<int>("FK_COURSEID");

                    b.Property<string>("InvitationHash")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("FK_APP_USER_RECIPIENTId");

                    b.HasIndex("FK_APP_USER_SENDERId");

                    b.HasIndex("FK_COURSEID");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("PeerIt.Models.PFile", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppUserId");

                    b.Property<string>("Ext");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("AppUserId");

                    b.ToTable("PFiles");
                });

            modelBuilder.Entity("PeerIt.Models.Review", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(100000);

                    b.Property<string>("FK_APP_USERId")
                        .IsRequired();

                    b.Property<int>("FK_STUDENT_ASSIGNMENTID");

                    b.Property<DateTime>("TimestampCreated");

                    b.HasKey("ID");

                    b.HasIndex("FK_APP_USERId");

                    b.HasIndex("FK_STUDENT_ASSIGNMENTID");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("PeerIt.Models.Setting", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(30);

                    b.Property<int>("NumericValue")
                        .HasMaxLength(20);

                    b.Property<string>("StringValue")
                        .HasMaxLength(1000);

                    b.HasKey("ID");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("PeerIt.Models.StudentAssignment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppUserId")
                        .IsRequired();

                    b.Property<string>("Content")
                        .HasMaxLength(100000);

                    b.Property<int>("CourseAssignmentID");

                    b.Property<int>("Score");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("TimestampCreated");

                    b.HasKey("ID");

                    b.HasIndex("AppUserId");

                    b.HasIndex("CourseAssignmentID");

                    b.ToTable("StudentAssignments");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("PeerIt.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PeerIt.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PeerIt.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("PeerIt.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PeerIt.Models.ActiveReviewer", b =>
                {
                    b.HasOne("PeerIt.Models.AppUser", "FK_APP_USER_VIEWER")
                        .WithMany()
                        .HasForeignKey("FK_APP_USER_VIEWERId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PeerIt.Models.StudentAssignment", "FK_STUDENT_ASSIGNMENT")
                        .WithMany()
                        .HasForeignKey("FK_STUDENT_ASSIGNMENTID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PeerIt.Models.Comment", b =>
                {
                    b.HasOne("PeerIt.Models.AppUser", "FK_APP_USER")
                        .WithMany()
                        .HasForeignKey("FK_APP_USERId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PeerIt.Models.StudentAssignment", "FK_STUDENT_ASSIGNMENT")
                        .WithMany()
                        .HasForeignKey("FK_STUDENT_ASSIGNMENTID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PeerIt.Models.Course", b =>
                {
                    b.HasOne("PeerIt.Models.AppUser", "FK_INSTRUCTOR")
                        .WithMany()
                        .HasForeignKey("FK_INSTRUCTORId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PeerIt.Models.CourseAssignment", b =>
                {
                    b.HasOne("PeerIt.Models.Course", "FK_COURSE")
                        .WithMany()
                        .HasForeignKey("FK_COURSEID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PeerIt.Models.CourseGroup", b =>
                {
                    b.HasOne("PeerIt.Models.AppUser", "FK_AppUser")
                        .WithMany()
                        .HasForeignKey("FK_AppUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PeerIt.Models.Course", "FK_Course")
                        .WithMany()
                        .HasForeignKey("FK_CourseID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PeerIt.Models.Event", b =>
                {
                    b.HasOne("PeerIt.Models.AppUser", "FK_AppUser")
                        .WithMany()
                        .HasForeignKey("FK_AppUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PeerIt.Models.ForgotPassword", b =>
                {
                    b.HasOne("PeerIt.Models.AppUser", "FK_APP_USER")
                        .WithMany()
                        .HasForeignKey("FK_APP_USERId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PeerIt.Models.Invitation", b =>
                {
                    b.HasOne("PeerIt.Models.AppUser", "FK_APP_USER_RECIPIENT")
                        .WithMany()
                        .HasForeignKey("FK_APP_USER_RECIPIENTId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PeerIt.Models.AppUser", "FK_APP_USER_SENDER")
                        .WithMany()
                        .HasForeignKey("FK_APP_USER_SENDERId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PeerIt.Models.Course", "FK_COURSE")
                        .WithMany()
                        .HasForeignKey("FK_COURSEID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PeerIt.Models.PFile", b =>
                {
                    b.HasOne("PeerIt.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("PeerIt.Models.Review", b =>
                {
                    b.HasOne("PeerIt.Models.AppUser", "FK_APP_USER")
                        .WithMany()
                        .HasForeignKey("FK_APP_USERId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PeerIt.Models.StudentAssignment", "FK_STUDENT_ASSIGNMENT")
                        .WithMany()
                        .HasForeignKey("FK_STUDENT_ASSIGNMENTID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PeerIt.Models.StudentAssignment", b =>
                {
                    b.HasOne("PeerIt.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PeerIt.Models.CourseAssignment", "CourseAssignment")
                        .WithMany()
                        .HasForeignKey("CourseAssignmentID")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}