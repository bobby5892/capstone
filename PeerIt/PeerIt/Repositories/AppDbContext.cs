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
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<ForgotPassword> ForgotPasswords { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<StudentAssignment> StudentAssignments { get; set; }
        public DbSet<CourseGroup> CourseGroups { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /* Don't mess with Cascade Delete */
            modelBuilder.Entity<CourseGroup>()
                .HasOne(c => c.FK_AppUser)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CourseGroup>()
                .HasOne(c => c.FK_Course)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invitation>()
                .HasOne(c => c.FK_APP_USER_RECIPIENT)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invitation>()
                .HasOne(c => c.FK_APP_USER_SENDER)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invitation>()
                .HasOne(c => c.FK_COURSE)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActiveReviewer>()
                .HasOne(c => c.FK_APP_USER_VIEWER)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActiveReviewer>()
                .HasOne(c => c.FK_STUDENT_ASSIGNMENT)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.FK_APP_USER)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.FK_STUDENT_ASSIGNMENT)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.FK_INSTRUCTOR)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CourseAssignment>()
                .HasOne(c => c.FK_COURSE)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasOne(c => c.FK_AppUser)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ForgotPassword>()
                .HasOne(c => c.FK_APP_USER)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(c => c.FK_APP_USER)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(c => c.FK_STUDENT_ASSIGNMENT)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentAssignment>()
                .HasOne(c => c.CourseAssignment)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentAssignment>()
                .HasOne(c => c.AppUser)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            
        }
        /*
         *modelBuilder.Entity<Course>()
            .HasMany<AppUser>(c => c.Students.)
            .WithOne(d => d.Id)



            .OnDelete(DeleteBehavior.Restrict);
         * 
         * protected override void OnModelCreating(ModelBuilder modelBuilder)
           {
               modelBuilder.Entity<CourseGroup>()
                   .HasMany("Course")
                       .WithOne("AppUser")
                   .OnDelete(DeleteBehavior.SetNull);

               modelBuilder.Entity<CourseGroup>()
                   .HasMany("AppUser")
                       .WithOne("Course")
                   .OnDelete(DeleteBehavior.SetNull);

           }
           */
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<BookCategory>()
        .HasKey(bc => new { bc.BookId, bc.CategoryId });  
    modelBuilder.Entity<BookCategory>()
        .HasOne(bc => bc.Book)
        .WithMany(b => b.BookCategories)
        .HasForeignKey(bc => bc.BookId);  
    modelBuilder.Entity<BookCategory>()
        .HasOne(bc => bc.Category)
        .WithMany(c => c.BookCategories)
        .HasForeignKey(bc => bc.CategoryId);
}
*/
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseGroup>()
                .HasKey(cg => new { cg.CourseID, cg.AppUserID });
            modelBuilder.Entity<CourseGroup>()
                .HasOne(cg => cg.FK_Course)
                .WithMany(c => c)
                .HasForeignKey(cg => cg.BookId);
            modelBuilder.Entity<CourseGroup>()
                .HasOne(cg => cg.Category)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(cg => cg.CategoryId);
        }*/
    }
}
