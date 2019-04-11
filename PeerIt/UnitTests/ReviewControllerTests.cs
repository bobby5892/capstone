using Xunit;
using PeerIt.Repositories;
using PeerIt.Controllers;
using PeerIt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PeerIt.ViewModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

        private Microsoft.AspNetCore.Identity.UserManager<AppUser> userManager;
        private ReviewController reviewController;
        private Review testReview;
        private JsonResult response;
        private string jResponse;

        public ReviewControllerTests()
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
            this.reviewController = new ReviewController(userManager, ReviewRepo);

            #region Dummy Data

            AppUser testAppUser = new AppUser() { Id = "1", FirstName = "Test" };
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
            await reviewController.CreateReview("TestReviewContents", "1", 2);
            Assert.True(ReviewRepo.FindByID(0).Content == "TestReviewContents");
        }
    }
}
