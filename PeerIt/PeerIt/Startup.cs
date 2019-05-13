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

using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Cors;

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
        protected virtual void AddCorsPolicy(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins("http://localhost:3000","http://localhost:8080")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            /* Enity Framework */
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:Local"]));
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            /* Add Repositories */
            services.AddTransient<IGenericRepository<ActiveReviewer,int>, ActiveReviewerRepository>();
            services.AddTransient<IGenericRepository<Comment, int>, CommentRepository>();
            services.AddTransient<IGenericRepository<CourseAssignment, int>, CourseAssignmentRepository>();
            services.AddTransient<IGenericRepository<CourseGroup, int>, CourseGroupRepository>();
            services.AddTransient<IGenericRepository<Course, int>, CourseRepository>();
            services.AddTransient<IGenericRepository<Event, int>, EventRepository>();
            services.AddTransient<IGenericRepository<ForgotPassword, int>, ForgotPasswordRepository>();
            services.AddTransient<IGenericRepository<Invitation, int>, InvitationRepository>();
            services.AddTransient<IGenericRepository<PFile,string>, PFileRepository>();
            services.AddTransient<IGenericRepository<Review, int>, ReviewRepository>();
            services.AddTransient<IGenericRepository<Setting, string>, SettingsRepository>();

            // Allow us to turn on and off cors - useful in development when building the react app - its dev env is on a different port then
            // the kestrel server - so its a cors violation - this allows me to circumvent it during coding
            // ********************
            // Setup CORS
            // ********************
            //app.UseCors(builder =>
            
            //var corsBuilder = new CorsPolicyBuilder();
            //corsBuilder.AllowAnyHeader();
            //corsBuilder.AllowAnyMethod();
            //corsBuilder.AllowAnyOrigin(); // For anyone access.
            //corsBuilder.WithOrigins("http://localhost:3000"); // for a specific url. Don't add a forward slash on the end!
            //corsBuilder.AllowCredentials();

            /*services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });*/
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

            /*
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/Login";
                options.Cookie.Name = "Peerit";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                options.LoginPath = "/Account/Login";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });*/

            //Fix a bug in cookies in core - https://github.com/aspnet/Identity/issues/1389
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                // enables immediate logout, after updating the user's stat.
                options.ValidationInterval = TimeSpan.FromHours(12);
            });

            //https://andrewlock.net/session-state-gdpr-and-non-essential-cookies/  (DISABLE GPDR in 2.2 - Stupid Microsoft!)
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //https://stackoverflow.com/questions/49276449/session-cookie-never-set-in-asp-net-core
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.Name = "Peerit";
                options.Cookie.IsEssential = true;
            });

        }
     
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("CorsPolicy");
            //app.UseCors(builder =>
            //    builder.WithOrigins("http://localhost:3000")
            //    .AllowAnyMethod()
            //    .AllowCredentials()
            //    .AllowAnyHeader()

            //);
            if (env.IsDevelopment())

            {
                // Must not exist in production - this disables cross side js checks
             /*   app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    );*/
                app.UseDeveloperExceptionPage();
                
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
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
           // This seems to break things app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();
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
