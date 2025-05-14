using CarShare.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShare.DAL.Pepository;
using CarShare.DAL.Pepository.UserRepository;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CarShare.DAL;
namespace CarShare.DAL
{
    using Microsoft.EntityFrameworkCore;

    public class CarShareDbContext : DbContext
    {
        public CarShareDbContext(DbContextOptions<CarShareDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<CarPost> CarPosts { get; set; }
        public DbSet<RentalRequest> RentalRequests { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enum mapping (UserType will be stored as int)
            modelBuilder.Entity<User>()
                .Property(u => u.UserType)
                .HasConversion<int>();

            // User - CarPosts (1 to many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.CarPosts)
                .WithOne(c => c.Owner)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // User - RentalRequests (1 to many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.RentalRequests)
                .WithOne(p => p.Renter)
                .HasForeignKey(p => p.RenterId)
                .OnDelete(DeleteBehavior.Restrict);

            // User - Feedbacks (1 to many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Feedbacks)
                .WithOne(f => f.Renter)
                .HasForeignKey(f => f.RenterId)
                .OnDelete(DeleteBehavior.Restrict);

            // CarPost - RentalRequests
            modelBuilder.Entity<CarPost>()
                .HasMany(c => c.RentalRequests)
                .WithOne(p => p.CarPost)
                .HasForeignKey(p => p.CarPostId)
                .OnDelete(DeleteBehavior.Cascade);

            // CarPost - Feedbacks
            modelBuilder.Entity<CarPost>()
                .HasMany(c => c.Feedbacks)
                .WithOne(f => f.CarPost)
                .HasForeignKey(f => f.CarPostId)
                .OnDelete(DeleteBehavior.Cascade);

        
            modelBuilder.Entity<CarPost>()
                .Property(c => c.RentalStatus)
                .HasConversion<string>();


            modelBuilder.Entity<CarPost>()
                .Property(c => c.Transmission)
                .HasConversion<string>();
        }

    
}
}
