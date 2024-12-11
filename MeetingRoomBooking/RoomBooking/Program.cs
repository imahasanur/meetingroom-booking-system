using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Infrastructure;
using RoomBooking.Infrastructure.Membership;
using Serilog;
using Serilog.Events;
using System.Configuration;
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

                //builder.Services.AddDbContext<ApplicationDbContext>(options =>
                //    options.UseSqlServer(connectionString,
                //    (m) => m.MigrationsAssembly(migrationAssembly)));

                builder.Services.AddScoped<ApplicationDbContext>(s => new ApplicationDbContext(connectionString, migrationAssembly));
                builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddUserManager<ApplicationUserManager>()
                   .AddRoleManager<ApplicationRoleManager>()
                   .AddSignInManager<ApplicationSignInManager>()
                   .AddDefaultTokenProviders();
                //builder.Services.AddScoped<SignInManager<ApplicationUser>>();
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();

                builder.Services.AddControllersWithViews();

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
