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
        public DbSet<Designation> Designations { get; set; }
        public DbSet<DesignationProduct> DesignationProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique indexes
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

            // Quotation-Designation relationship (One-to-Many)
            modelBuilder.Entity<Quotation>()
                .HasMany(q => q.Designations)
                .WithOne(d => d.Quotation)
                .HasForeignKey(d => d.QuotationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Designation-Product relationship (Many-to-Many via DesignationProduct)
            modelBuilder.Entity<DesignationProduct>()
                .HasKey(dp => new { dp.DesignationId, dp.ProductId });

            modelBuilder.Entity<DesignationProduct>()
                .HasOne(dp => dp.Designation)
                .WithMany(d => d.DesignationProducts)
                .HasForeignKey(dp => dp.DesignationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DesignationProduct>()
                .HasOne(dp => dp.Product)
                .WithMany(p => p.DesignationProducts)
                .HasForeignKey(dp => dp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quotation-NormalCustomer relationship (One-to-Many)
            modelBuilder.Entity<Quotation>()
                .HasOne(q => q.NormalCustomer)
                .WithMany(nc => nc.Quotations)
                .HasForeignKey(q => q.NormalCustomerId)
                .IsRequired(false) // Optional relationship
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of customer if quotations exist

            // Quotation-CompanyCustomer relationship (One-to-Many)
            modelBuilder.Entity<Quotation>()
                .HasOne(q => q.CompanyCustomer)
                .WithMany(cc => cc.Quotations)
                .HasForeignKey(q => q.CompanyCustomerId)
                .IsRequired(false) // Optional relationship
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of customer if quotations exist

            // Default values for PostgreSQL
            modelBuilder.Entity<Quotation>()
                .Property(q => q.CreatedAt)
                .HasDefaultValueSql("NOW()"); // PostgreSQL function for current UTC timestamp

            modelBuilder.Entity<Designation>()
                .Property(d => d.CreatedAt)
                .HasDefaultValueSql("NOW()"); // PostgreSQL function for current UTC timestamp

            // Optional: Ensure CustomerType is stored as desired (default is int, can be string if preferred)
            modelBuilder.Entity<Quotation>()
                .Property(q => q.CustomerType)
                .HasConversion<string>(); // Stores enum as string (e.g., "NORMAL", "COMPANY") instead of int (0, 1)
        }
    }
}