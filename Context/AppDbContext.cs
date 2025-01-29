using Microsoft.EntityFrameworkCore;
using SmartCafeOrderingSystem_Api_V2.Models;

namespace SmartCafeOrderingSystem_Api_V2.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<Analytics> Analytics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recommendation>()
                .HasOne(r => r.Item1)
                .WithMany()
                .HasForeignKey(r => r.ItemID1)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Recommendation>()
                .HasOne(r => r.Item2)
                .WithMany()
                .HasForeignKey(r => r.ItemID2)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }

}
