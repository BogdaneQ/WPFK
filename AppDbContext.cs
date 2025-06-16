using Microsoft.EntityFrameworkCore;
using WPFK.Models;

namespace WPFK.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<ParcelStatusHistory> ParcelStatusHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Parcel>()
                .HasOne(p => p.Sender)
                .WithMany() // Optionally: .WithMany(u => u.SentParcels)
                .HasForeignKey(p => p.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Parcel>()
                .HasOne(p => p.Recipient)
                .WithMany() // Optionally: .WithMany(u => u.ReceivedParcels)
                .HasForeignKey(p => p.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

           
            modelBuilder.Entity<ParcelStatusHistory>()
                .HasOne(psh => psh.Parcel)
                .WithMany(p => p.StatusHistories)
                .HasForeignKey(psh => psh.ParcelId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=ep-sweet-fog-a8cvnz9p-pooler.eastus2.azure.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_Qrsg19xtqPOb");
            }
        }
    }
}
