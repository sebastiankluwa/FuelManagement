using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
    

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Refueling> Refuelings { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Refueling>()
                .HasKey(k => new { k.VehicleId, k.RefuelDate });

            builder.Entity<Refueling>()
                .HasOne(p => p.Tank)
                .WithMany(b => b.Refuelings)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Refueling>()
                .HasOne(p => p.Vehicle)
                .WithMany(b => b.Refuelings)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Refueling>()
                .HasOne(p => p.AppUser)
                .WithMany(b => b.Refuelings)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}