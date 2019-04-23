/*using Xunit;
using PeerIt.Repositories;
using PeerIt.Controllers;
using PeerIt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PeerIt.ViewModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Moq;
using UnitTests.Mock;

namespace UnitTests
{
    public class ReviewControllerTests
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

        private ReviewController reviewController;
        private Review testReview;

        private AppUser testAppUser;
        private string testAppUserPassword;

        private JsonResult response;
        private string jResponse;

        private MockUserManager userManager;

        public ReviewControllerTests()
        {

            //This is a Mocked User Manager
           userManager = new MockUserManager();   


            
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
            this.reviewController = new ReviewController(userManager, ReviewRepo);

            #region Dummy Data
           // Start Creating a User
            testAppUser = new AppUser()
            {
                FirstName = "Test",
                LastName = "User",
                Email = "ted@example.com"
            };
            // Hash a Password for the user
            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<AppUser>();
            // Clear text version of password
            string clearTextPassword = "test";
            // Hashed Password using the User Salt
            string hashedPassword = hasher.HashPassword(testAppUser, clearTextPassword);
            // Set the New users password to the hash
            testAppUser.PasswordHash = hashedPassword;


            CreateFakeUser();
            Course course = new Course() { ID = 5, FK_INSTRUCTOR = testAppUser, IsActive = true };
            CourseAssignment courseAssignment = new CourseAssignment() { ID = 4, FK_COURSE = course, };
            StudentAssignment studentAssignment = new StudentAssignment() { ID = 2, AppUser = testAppUser, CourseAssignment = courseAssignment,};
            testReview = new Review() { ID = 6, FK_APP_USER = testAppUser, FK_STUDENT_ASSIGNMENT = studentAssignment, Content = "Lots of content" };

            #endregion
        }

        [Fact]
        public void GetReviewByAssignmentIdTest()
        {
            ReviewRepo.Add(testReview);
            int assignmentId = 2;
            response = reviewController.GetReviewsByAssignmentId(assignmentId);

            jResponse = JsonConvert.SerializeObject(response.Value);
            JsonResponse<Review> review = JsonConvert.DeserializeObject<JsonResponse<Review>>(jResponse);
            Assert.True(review.Data[0].ID == 6);  
        }
        [Fact]
        public void GetReviewByIdTestWithValidId()
        {
            ReviewRepo.Add(testReview);
            int reviewId = 6;

            response = reviewController.GetReviewById(reviewId);
            jResponse = JsonConvert.SerializeObject(response.Value);
            JsonResponse<Review> review = JsonConvert.DeserializeObject<JsonResponse<Review>>(jResponse);

            Assert.True(review.Data[0].FK_STUDENT_ASSIGNMENT.ID == 2);
        }
        [Fact]
        public void GetReviewByIdTestWithInValidId()
        {
            ReviewRepo.Add(testReview);
            int reviewId = 5;

            response = reviewController.GetReviewById(reviewId);
            jResponse = JsonConvert.SerializeObject(response.Value);
            JsonResponse<Review> review = JsonConvert.DeserializeObject<JsonResponse<Review>>(jResponse);

            Assert.True(review.Error[0].Name == "No Review");
        }
        [Fact]
        public async void CreateReviewTest()
        {
            string id = testAppUser.Id;
            response = await reviewController.CreateReview("TestReviewContents", id, 2);
            Assert.True(ReviewRepo.FindByID(0).Content == "TestReviewContents");
        }

        internal async void CreateFakeUser()
        {
            await userManager.CreateAsync(testAppUser);
        }     
    }
}
*/