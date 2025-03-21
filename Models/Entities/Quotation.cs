using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using DHP_Manager.Models.ENUM;

namespace DHP_Manager.Models.Entities
{
    public class Quotation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string ProjectName { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public CustomerType CustomerType { get; set; }

        [Required]
        public QuotationStatus Status { get; set; } = QuotationStatus.PENDING; // New field with default value

        // Foreign key for NormalCustomer (nullable)
        public int? NormalCustomerId { get; set; }
        public NormalCustomer NormalCustomer { get; set; }

        // Foreign key for CompanyCustomer (nullable)
        public int? CompanyCustomerId { get; set; }
        public CompanyCustomer CompanyCustomer { get; set; }

        // One Quotation -> Many Designations
        public List<Designation> Designations { get; set; } = new();
    }
}