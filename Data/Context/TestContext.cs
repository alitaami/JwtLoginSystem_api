using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.TestContext
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options)
        : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                UserName = "ali",
                Email = "sample1@example.com",
                PasswordHash = "8jr0nptvznD5VS2WniCx5y6jYyQOSw1ZpfsulA8c/3A=",
                FullName = "Sample User One",
                Age = 30
            },
            new User
            {
                Id = 2,
                UserName = "ata",
                Email = "sample2@example.com",
                PasswordHash = "8jr0nptvznD5VS2WniCx5y6jYyQOSw1ZpfsulA8c/3A=",
                FullName = "Sample User Two",
                Age = 25
            }
            // ... add more users if needed
        );
            modelBuilder.Entity<RefreshToken>().HasData(
           new RefreshToken
           {
               Id = 2,
               Token = "D66709F8-27E9-4164-AF91-CA0877F10FEF",
               UserId = 1,
               IssuedAt = DateTime.Parse("2023-09-26 01:31:34.4433333"),
               ExpiresAt = DateTime.Parse("2023-10-11 01:31:34.4433333"),
               IsUsed = false,
               IsRevoked = false
           },
           new RefreshToken
           {
               Id = 3,
               Token = "67C8F733-BF6A-4E49-8109-522DACCC60F7",
               UserId = 2,
               IssuedAt = DateTime.Parse("2023-09-26 01:31:34.4433333"),
               ExpiresAt = DateTime.Parse("2023-10-11 01:31:34.4433333"),
               IsUsed = false,
               IsRevoked = false
           }
       // ... add more tokens if needed
       );
        }
    }
}
