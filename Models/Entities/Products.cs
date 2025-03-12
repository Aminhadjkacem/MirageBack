using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DHP_Manager.Models.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Price { get; set; }

        [Required]
        public required int Stock { get; set; }

        [Required]
        [StringLength(50)]
        public required string Category { get; set; }

        // ✅ Added Navigation Property for Many-to-Many Relationship
        public List<QuotationProduct> QuotationProducts { get; set; } = new();
    }
}
