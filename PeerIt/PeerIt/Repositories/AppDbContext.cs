using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using PeerIt.Models;
namespace PeerIt.Repositories
{
    public class AppDBContext : IdentityDbContext<AppUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
        public DbSet<ActiveReviewer> ActiveReviewers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<CourseGroup> CourseGroups { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<ForgotPassword> ForgotPasswords { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<StudentAssignment> StudentAssignments { get; set; }
    }
}
