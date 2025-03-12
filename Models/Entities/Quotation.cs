using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DHP_Manager.Models.Entities
{
    public class Quotation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Designation { get; set; }

        [Required]
        [StringLength(100)]
        public required string ProjectName { get; set; }

        // Navigation property for the many-to-many relationship
        public List<QuotationProduct> QuotationProducts { get; set; } = new();
    }
}
