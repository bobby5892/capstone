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
using Microsoft.AspNetCore.Cors;
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
            /* Enity Framework */
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:Local"]));
            
            /* Add Repositories */
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
            services.AddTransient<IGenericRepository<Setting, string>, SettingsRepository>(); services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            // Allow us to turn on and off cors - useful in development when building the react app - its dev env is on a different port then
            // the kestrel server - so its a cors violation - this allows me to circumvent it during coding
            services.AddCors();
            /* Users */
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:Local"]));
            services.AddIdentity<AppUser, IdentityRole>(opts => {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppDBContext>()

        .AddDefaultTokenProviders();
            // Secondary Loading of ANTI CORS - NEEDS REMOVED BEFORE PRODUCTION
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
   
        }
     
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

           if (env.IsDevelopment())

            {
                // Must not exist in production - this disables cross side js checks
                app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    
                    );
                app.UseDeveloperExceptionPage();
                
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseSession();
            // https://github.com/aspnet/Identity/issues/1065
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var signInManager = serviceScope.ServiceProvider.GetService<SignInManager<AppUser>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
                //https://stackoverflow.com/questions/32459670/resolving-instances-with-asp-net-core-di
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            //AppDBContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
            app.UseMvc(routes =>
            {
                //Custom route for getting single complaint
                /* routes.MapRoute(
                    "GetComplaint",
                    "Complaint/Get/{id}",
                    new { controller = "Complaint", action = "GetComplaint" }
                 );
                 */
                 routes.MapRoute(
                    "GetSetting",
                    "Setting/GetSetting/{id}",
                    new { controller = "Settings", action = "GetSetting" }
                 );
                routes.MapRoute(
                    "EditSetting",
                    "Setting/EditSetting/{id}",
                    new { controller = "Settings", action = "EditSetting" }
                );
                routes.MapRoute(
                    "GetSettings",
                    "Setting/",
                    new { controller = "Settings", action = "GetSettings" }
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
