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
        public DbSet<Product> Products { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<QuotationProduct> QuotationProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NormalCustomer>()
                .HasIndex(c => c.CIN)
                .IsUnique();

            modelBuilder.Entity<CompanyCustomer>()
                .HasIndex(c => c.MatriculeFiscal)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.mail)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelBuilder.Entity<Quotation>()
                .HasIndex(u => u.Designation)
                .IsUnique();

            // Define Many-to-Many Relationship for Quotation and Product
            modelBuilder.Entity<QuotationProduct>()
                .HasOne(qp => qp.Quotation)
                .WithMany(q => q.QuotationProducts)
                .HasForeignKey(qp => qp.QuotationId)
                .OnDelete(DeleteBehavior.Cascade); // Ensures cascading delete when a quotation is removed

            modelBuilder.Entity<QuotationProduct>()
    .HasOne(qp => qp.Product)
    .WithMany(p => p.QuotationProducts) // ✅ Ajoute la navigation inverse
    .HasForeignKey(qp => qp.ProductId)
    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
