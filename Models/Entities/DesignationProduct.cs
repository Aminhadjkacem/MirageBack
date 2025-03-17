using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DHP_Manager.Models.Entities
{
    public class DesignationProduct
    {
        [Key]
        public int Id { get; set; }

        // Foreign Keys
        public int DesignationId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // Navigation Properties
        [ForeignKey("DesignationId")]
        public Designation Designation { get; set; } = null!;

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;
    }
}
