using AracKiralamaAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AracKiralamaAPI.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Vehicle>  Vehicles   { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Rental>   Rentals    { get; set; }
        public DbSet<Location> Locations  { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Çoklu FK — döngüsel silme hatasını önle
            builder.Entity<Rental>()
                .HasOne(r => r.PickupLocation)
                .WithMany(l => l.PickupRentals)
                .HasForeignKey(r => r.PickupLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Rental>()
                .HasOne(r => r.DropoffLocation)
                .WithMany(l => l.DropoffRentals)
                .HasForeignKey(r => r.DropoffLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Decimal precision
            builder.Entity<Vehicle>()
                .Property(v => v.DailyPrice).HasColumnType("decimal(18,2)");

            builder.Entity<Rental>()
                .Property(r => r.TotalPrice).HasColumnType("decimal(18,2)");

            // Unique index
            builder.Entity<Vehicle>()
                .HasIndex(v => v.PlateNumber).IsUnique();
        }
    }
}
