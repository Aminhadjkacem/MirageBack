using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public CustomerType CustomerType { get; set; }

        [Required]
        public QuotationStatus Status { get; set; } = QuotationStatus.PENDING;

        [Required]
        public decimal TotalPrice { get; set; } // New field to persist total price

        [Required]
        public decimal TotalPriceHT { get; set; } // New field to persist total price hors taxe

        // Foreign key for NormalCustomer (nullable)
        public int? NormalCustomerId { get; set; }
        public NormalCustomer NormalCustomer { get; set; }

        // Foreign key for CompanyCustomer (nullable)
        public int? CompanyCustomerId { get; set; }
        public CompanyCustomer CompanyCustomer { get; set; }

        // One Quotation -> Many Designations
        public List<Designation> Designations { get; set; } = new();

        // New fields
        public int? LastInvoiceId { get; set; } // Nullable foreign key for the last invoice

        public DateTime? InvoiceCreatedAt { get; set; } // Nullable date for invoice creation

        public DateTime? QuotationCreatedAt { get; set; } // Nullable date for quotation creation
    }
}
