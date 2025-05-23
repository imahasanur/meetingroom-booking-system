﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RoomBooking.Domain.Domain.Entities;
using RoomBooking.Infrastructure.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RoomBooking.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,
        ApplicationRole, Guid,
        ApplicationUserClaim, ApplicationUserRole,
        ApplicationUserLogin, ApplicationRoleClaim,
        ApplicationUserToken>, IApplicationDbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public ApplicationDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString, x => x.MigrationsAssembly(_migrationAssembly));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminPassword = Environment.GetEnvironmentVariable("AdminPassword");
            var adminIdentity = Environment.GetEnvironmentVariable("AdminIdentification");
            var adminIdentityUpper = Environment.GetEnvironmentVariable("AdminIdentificationUpper");
            var adminFirstName = Environment.GetEnvironmentVariable("AdminFirstName");
            var adminLastName = Environment.GetEnvironmentVariable("AdminLastName");
            var adminFullName = Environment.GetEnvironmentVariable("AdminFullName");

            var passwordHasher = new PasswordHasher<ApplicationUser>();

            var adminUserId = new Guid("6C23FDA7-AE43-439E-B7F9-7D30868CB399");
            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = adminIdentity,
                NormalizedUserName = adminIdentityUpper,
                Email = adminIdentity,
                NormalizedEmail = adminIdentityUpper,
                EmailConfirmed = true,
                SecurityStamp = string.Empty,
                FirstName = adminFirstName,
                LastName = adminLastName,
                FullName = adminFullName,
                MemberPin = "1122",
                Department = "Developer",
                CreatedAtUTC = new DateTime(2024, 12, 11, 0, 0, 0, DateTimeKind.Utc)
            };
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, adminPassword);

            builder.Entity<ApplicationUser>().HasData(adminUser);

            builder.Entity<ApplicationUserClaim>().HasData(
                new ApplicationUserClaim
                {
                    Id = 1,
                    UserId = adminUserId,
                    ClaimType = "role",
                    ClaimValue = "admin"
                },
                new ApplicationUserClaim
                {
                    Id = 2,
                    UserId = adminUserId,
                    ClaimType = "role",
                    ClaimValue = "user"
                }
            );

            builder.Entity<ApplicationUser>().Property(x => x.MemberPin).HasMaxLength(20);

            builder.Entity<Room>().Property(p => p.ConcurrencyToken).IsConcurrencyToken();

            builder.Entity<Event>()
                .HasMany(e => e.Guests)          
                .WithOne()                       
                .HasForeignKey(g => g.EventId)    
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Room>()
                .HasMany(e => e.Events)
                .WithOne()
                .HasForeignKey(e => e.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Guest>()
                .HasOne(g => g.Event)                
                .WithMany(e => e.Guests)             
                .HasForeignKey(g => g.EventId)       
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Event>()
                .HasOne(e => e.Room)
                .WithMany(x => x.Events)
                .HasForeignKey(k => k.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EventTime>().HasData(new EventTime
            {
                Id = new Guid("6617251D-F526-40CD-B58F-7F2BFB50E8F6"),
                MinimumTime = 15,
                MaximumTime = 120,
                CreatedAtUTC = new DateTime(2024, 12, 22, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = adminIdentity,
            });

        }

        public DbSet<Room> Room { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Guest> Guest { get; set; }
        public DbSet<EventTime> Time { get; set; }
        public DbSet<UploadedUser> UploadedUser { get; set; }
        public DbSet<ApplicationUser> IdentityUser { get; set; }
    }
}
