using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using RoomBooking.Application;
using RoomBooking.Application.Domain.Repositories;
using RoomBooking.Application.Services;
using RoomBooking.Application.Services.Booking;
using RoomBooking.Application.Services.Room;
using RoomBooking.Infrastructure;
using RoomBooking.Infrastructure.Membership;
using RoomBooking.Infrastructure.Repositories;
using RoomBooking.Models.Room;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace RoomBooking
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Env.Load();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateBootstrapLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Configuration
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables();
                builder.Host.UseSerilog((ctx, lc) => lc
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .ReadFrom.Configuration(builder.Configuration));

                var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DbCon");
                var migrationAssembly = Assembly.GetExecutingAssembly().FullName;

                builder.Services.AddScoped<ApplicationDbContext>(s => new ApplicationDbContext(connectionString, migrationAssembly));
                builder.Services.AddScoped<IApplicationDbContext>(s => new ApplicationDbContext(connectionString, migrationAssembly));
                builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddUserManager<ApplicationUserManager>()
                   .AddRoleManager<ApplicationRoleManager>()
                   .AddSignInManager<ApplicationSignInManager>()
                   .AddDefaultTokenProviders();

                builder.Services.AddScoped<IBookingManagementService, BookingManagementService>();
                builder.Services.AddScoped<IRoomManagementService, RoomManagementService>();

                builder.Services.AddScoped<IBookingRepository, BookingRepository>();
                builder.Services.AddScoped<IRoomRepository, RoomRepository>();
                builder.Services.AddScoped<IGuestRepository, GuestRepository>();
                builder.Services.AddScoped<IApplicationUnitOfWork, ApplicationUnitOfWork>();
                builder.Services.AddScoped<ApplicationUser>();

                builder.Services.AddScoped<GetAllRoomViewModel>();
                builder.Services.AddScoped<EditRoomViewModel>();

                builder.Services.AddDatabaseDeveloperPageExceptionFilter();

                builder.Services.AddControllersWithViews();
                builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
                {
                    x.LoginPath = "/account/Login";
                    x.AccessDeniedPath = "/account/AccessDenied";
                    x.LogoutPath = "/";
                    x.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                });

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseMigrationsEndPoint();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //app.MapRazorPages();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to start the application.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
