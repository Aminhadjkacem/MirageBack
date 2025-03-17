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
        public required string ProjectName { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ✅ One Quotation -> Many Designations
        public List<Designation> Designations { get; set; } = new();
    }
}
