using DHP_Manager.Entities;
using DHP_Manager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DHP_Manager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<NormalCustomer> NormalCustomers { get; set; }
        public DbSet<CompanyCustomer> CompanyCustomers { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NormalCustomer>()
                .HasIndex(c => c.CIN)
                .IsUnique();

            modelBuilder.Entity<CompanyCustomer>()
                .HasIndex(c => c.MatriculeFiscal)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
