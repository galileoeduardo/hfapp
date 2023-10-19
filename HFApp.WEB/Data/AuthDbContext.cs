using HFApp.WEB.Models.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace HFApp.WEB.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var roles = new List<IdentityRole>() {

              new IdentityRole() { Id = "3860006c-5e47-4ad8-b9a0-f366ecc81926", Name = "SuperUser", NormalizedName = "SUPERUSER", ConcurrencyStamp = "3860006c-5e47-4ad8-b9a0-f366ecc81926" },
              new IdentityRole() { Id = "eb7a984d-c924-4884-a3c5-4d881bf4a0b7", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "eb7a984d-c924-4884-a3c5-4d881bf4a0b7" },
              new IdentityRole() { Id = "58314391-c7b2-4b1c-b8d7-8a31ec4abe8d", Name = "User", NormalizedName = "USER", ConcurrencyStamp = "58314391-c7b2-4b1c-b8d7-8a31ec4abe8d" }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            var users = new List<IdentityUser>() {
                new IdentityUser()
                {
                    Id = "9317279f-639b-420f-8a9c-363a4cacabc5",
                    UserName = "sa",
                    Email = "sa@email.com",
                    NormalizedUserName = "SA",
                    NormalizedEmail = "SA@EMAIL.COM"
                },
                new IdentityUser()
                {
                    Id = "6b50e04c-323b-4f11-88c4-c15d8aedac92",
                    UserName = "josuel.lopes",
                    Email = "josuel.lopes@email.com",
                    NormalizedUserName = "JOSUEL.LOPES",
                    NormalizedEmail = "JOSUEL.LOPES@EMAIL.COM"
                },
                new IdentityUser()
                {
                    Id = "b940e4f1-daf1-49af-ab92-f2d25d1e8dc6",
                    UserName = "eduardo.vieira",
                    Email = "eduardo.vieira@email.com",
                    NormalizedUserName = "EDUARDO.VIEIRA",
                    NormalizedEmail = "EDUARDO.VIEIRA@EMAIL.COM"
                }
            };
            users[0].PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(users[0], "sa@123");
            users[1].PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(users[1], "jl@123");
            users[2].PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(users[2], "ev@123");

            builder.Entity<IdentityUser>().HasData(users);

            var userRolesSA = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>()
                {
                    RoleId = roles[0].Id,
                    UserId  = users[0].Id
                },
                new IdentityUserRole<string>()
                {
                    RoleId = roles[1].Id,
                    UserId  = users[0].Id
                },
                new IdentityUserRole<string>()
                {
                    RoleId = roles[2].Id,
                    UserId  = users[0].Id
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(userRolesSA);

        }
    }
}
