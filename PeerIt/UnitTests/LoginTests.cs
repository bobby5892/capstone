/*using System;
using Xunit;
using PeerIt.Repositories;
using PeerIt.Models;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using UnitTests.Mock;
namespace UnitTests
{
    public class LoginTests
    {
        #region repoAccess
        public AppDBContext Context { get; set; }

        public ActiveReviewerRepository ActivewReviewerRepo { get; set; }
        public CommentRepository CommentRepo { get; set; }
        public CourseAssignmentRepository CourseAssignmentRepo { get; set; }
        public CourseGroupRepository CourseGroupRepo { get; set; }
        public EventRepository EventRepo { get; set; }
        public ForgotPasswordRepository ForgotPasswordRepo { get; set; }
        public InvitationRepository InvitationRepo { get; set; }
        public ReviewRepository ReviewRepo { get; set; }
        public SettingsRepository SettingsRepo { get; set; }
        #endregion
        private MockUserManager userManager;
        private MockRoleManager roleManager;


        public LoginTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "temp")
                .Options;
            //Mock user manager and role manager
            this.userManager = new MockUserManager();
            this.roleManager = new MockRoleManager();   
            #region repoInstance
            Context = new AppDBContext(optionsBuilder);
            this.ActivewReviewerRepo = new ActiveReviewerRepository(this.Context);
            this.CommentRepo = new CommentRepository(this.Context);
            this.CourseAssignmentRepo = new CourseAssignmentRepository(this.Context);
            this.CourseGroupRepo = new CourseGroupRepository(this.Context);
            this.EventRepo = new EventRepository(this.Context);
            this.ForgotPasswordRepo = new ForgotPasswordRepository(this.Context);
            this.InvitationRepo = new InvitationRepository(this.Context);
            this.ReviewRepo = new ReviewRepository(this.Context);
            this.SettingsRepo = new SettingsRepository(this.Context);
            #endregion
           

        }
        [Fact]
        public void Test1()
        {
            Assert.True(true);


        }
        [Fact]
        public void Test()
        {
                     //              "AQAAAAEAACcQAAAAECOsH8Nrr4kIt+J+quzqMSXx6fYN55IHg2QPdBFpa+diUvw5lwNpBD/1SvBkQMfZnA=="
            string AdminPassHash = "AQAAAAEAACcQAAAAEMn5kWqhcNiZifbhS26T13GmnIGnnpL4mCbd7psWNq5Zgjo4ucQNYJcKtMn9N0iuTQ==";
            AppUser admin = new AppUser()
            {
                Id = "8820ee66-681f-45b3-92ed-7d8dee5d4beb",
                FirstName = "Admin",
                LastName = "Admin",
                SecurityStamp = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("eargFRergijdfoaij34g34")),
                Email = "admin@example.com",
                NormalizedEmail = "admin@example.com",
                LockoutEnabled = false,
                IsEnabled = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                TimestampCreated = DateTime.Now
            };

            var thing = userManager.MockPasswordHash(admin, "password");


        }



    }
}

*/