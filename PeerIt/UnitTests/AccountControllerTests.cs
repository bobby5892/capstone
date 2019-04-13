using Xunit;
using PeerIt.Repositories;
using PeerIt.Controllers;
using PeerIt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PeerIt.ViewModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace UnitTests
{
    class AccountControllerTests
    {
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
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private RoleManager<IdentityRole> roleManager;
        private AccountController accountController;
        private AppUser appUser;
        private JsonResult response;
        private string jResponse;

        public AccountControllerTests()
        {
            /* Create a in Memory Database instead of using the SQL  - Destroyed after running*/
            var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "temp")
                .Options;

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
            this.accountController = new AccountController(userManager, signInManager, roleManager);
            

        }
    }
}
