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

namespace UnitTests
{
    public class CommentControllerTests
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

        public CommentControllerTests()
        {
          
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
        }
        [Fact]
        public void GetCommentByUserIdTest()
        {

        }
        [Fact]
        public void GetCommentByAssignmentIdTest()
        {

        }
        [Fact]
        public void GetCommentCountForUserTest()
        {

        }
        [Fact]
        public void GetCommentCountForAssignmentTest()
        {

        }
        [Fact]
        public void CreateCommentTest()
        {
            
        }
        [Fact]
        public void DeleteCommentTest()
        {

        }
    }
}
*/