using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DHP_Manager.Models.Entities
{
    public class Designation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public int QuotationId { get; set; }

        [ForeignKey("QuotationId")]
        public Quotation Quotation { get; set; } = null!;

        public List<DesignationProduct> DesignationProducts { get; set; } = new();

        // ✅ Add the missing CreatedAt field
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
