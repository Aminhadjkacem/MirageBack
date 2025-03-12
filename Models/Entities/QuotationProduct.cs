using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DHP_Manager.Models.Entities
{
    public class QuotationProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuotationId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        // ✅ Explicitly define relationships
        [ForeignKey("QuotationId")]
        public Quotation Quotation { get; set; } = null!;

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;
    }
}
