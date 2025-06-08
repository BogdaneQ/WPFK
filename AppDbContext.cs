using Microsoft.EntityFrameworkCore;
using WPFK.Models;

namespace WPFK.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<ParcelStatusHistory> ParcelStatusHistories { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=ep-polished-block-a8hgsg6l-pooler.eastus2.azure.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_CEN4rXGsAS9D");
            }
        }
    }
}
