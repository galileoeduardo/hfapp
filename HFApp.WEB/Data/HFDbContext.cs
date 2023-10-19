using HFApp.WEB.Models.Domain.Entities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace HFApp.WEB.Data
{
    public class HFDbContext : DbContext
    {
        public HFDbContext(DbContextOptions<HFDbContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> UserEntities { get; set; }
        public DbSet<FileEntity> FileEntities { get; set; }
        public DbSet<MineTypesEntity> MineTypesEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>().HasData(
              new UserEntity() { Id = 1, CreatedAt = DateTime.UtcNow, IdentityUserId = new Guid("9317279f-639b-420f-8a9c-363a4cacabc5") },
              new UserEntity() { Id = 2, CreatedAt = DateTime.UtcNow,  IdentityUserId = new Guid("9317279f-639b-420f-8a9c-363a4cacabc5") },
              new UserEntity() { Id = 3, CreatedAt = DateTime.UtcNow, IdentityUserId = new Guid("9317279f-639b-420f-8a9c-363a4cacabc5") }
           );

            modelBuilder.Entity<MineTypesEntity>().HasData(
              new MineTypesEntity() { Id = 1, CreatedAt = DateTime.UtcNow, Extension = ".aac", Kind = "AAC audio", Type = "audio/aac" },
              new MineTypesEntity() { Id = 2, CreatedAt = DateTime.UtcNow, Extension = ".bmp", Kind = "Windows OS/2 Bitmap Graphics", Type = "image/bmp" },
              new MineTypesEntity() { Id = 3, CreatedAt = DateTime.UtcNow, Extension = ".csv", Kind = "Comma - separated values(CSV)", Type = "text/csv" },
              new MineTypesEntity() { Id = 4, CreatedAt = DateTime.UtcNow, Extension = ".doc", Kind = "Microsoft Word", Type = "application/msword" },
              new MineTypesEntity() { Id = 5, CreatedAt = DateTime.UtcNow, Extension = ".docx", Kind = "Microsoft Word(OpenXML)", Type = "application / vnd.openxmlformats - officedocument.wordprocessingml.document" },
              new MineTypesEntity() { Id = 6, CreatedAt = DateTime.UtcNow, Extension = ".jpg", Kind = "JPEG images", Type = "image/jpeg" },
              new MineTypesEntity() { Id = 7, CreatedAt = DateTime.UtcNow, Extension = ".jpeg", Kind = "JPEG images", Type = "image/jpeg" },
              new MineTypesEntity() { Id = 8, CreatedAt = DateTime.UtcNow, Extension = ".xls", Kind = "Microsoft Excel", Type = "application/vnd.ms-excel" },
              new MineTypesEntity() { Id = 9, CreatedAt = DateTime.UtcNow, Extension = ".xlsx", Kind = "Microsoft Excel (OpenXML)", Type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
              new MineTypesEntity() { Id = 10, CreatedAt = DateTime.UtcNow, Extension = ".json", Kind = "JSON format", Type = "application/json" },
              new MineTypesEntity() { Id = 11, CreatedAt = DateTime.UtcNow, Extension = ".png", Kind = "Portable Network Graphics", Type = "image/png" },
              new MineTypesEntity() { Id = 12, CreatedAt = DateTime.UtcNow, Extension = ".pdf", Kind = "Adobe Portable Document Format(PDF)", Type = "application/pdf" },
              new MineTypesEntity() { Id = 13, CreatedAt = DateTime.UtcNow, Extension = ".xml", Kind = "XML", Type = "application/xml" }
           );

            modelBuilder.Entity<MineTypesEntity>()
            .HasMany(e => e.File)
            .WithOne(e => e.MineTypes)
            .HasForeignKey(e => e.MineTypesId)
            .IsRequired();
        }   
    }
}
