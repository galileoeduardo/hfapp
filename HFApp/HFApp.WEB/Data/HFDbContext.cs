using HFApp.WEB.Models.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HFApp.WEB.Data
{
    public class HFDbContext : DbContext
    {
        public HFDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserEntity> UserEntities { get; set; }
        public DbSet<RoleEntity> RoleEntities { get; set; }
    }
}
