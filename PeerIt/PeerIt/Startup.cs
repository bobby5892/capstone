using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using PeerIt.Models;
using PeerIt.Repositories;
using PeerIt.Interfaces;
/*
 *  Requires .net core 2.2  - https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.105-windows-x64-installer
 * 
 */
namespace PeerIt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:Local"]));

            services.AddTransient<IGenericRepository<ActiveReviewer,int>, ActiveReviewerRepository>();
            services.AddTransient<IGenericRepository<Comment, int>, CommentRepository>();
            services.AddTransient<IGenericRepository<CourseAssignment, int>, CourseAssignmentRepository>();
            services.AddTransient<IGenericRepository<CourseGroup, int>, CourseGroupRepository>();
            services.AddTransient<IGenericRepository<Course, int>, CourseRepository>();
            services.AddTransient<IGenericRepository<Event, int>, EventRepository>();
            services.AddTransient<IGenericRepository<ForgotPassword, int>, ForgotPasswordRepository>();
            services.AddTransient<IGenericRepository<CourseAssignment, int>, CourseAssignmentRepository>();
            services.AddTransient<IGenericRepository<Invitation, int>, InvitationRepository>();
            services.AddTransient<IGenericRepository<Review, int>, ReviewRepository>();
            services.AddTransient<IGenericRepository<Setting, string>, SettingsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
