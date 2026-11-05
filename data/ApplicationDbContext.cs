
using JeevanSetu.Models;
using Microsoft.EntityFrameworkCore;

namespace JeevanSetu.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Donor> Donors { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<BloodInventory> BloodInventories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Donor)
                .WithMany()
                .HasForeignKey(a => a.DonorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BloodInventory>()
                .HasOne(b => b.Hospital)
                .WithMany()
                .HasForeignKey(b => b.HospitalId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
