using System.ComponentModel.DataAnnotations;

namespace DHP_Manager.Models.Entities
{
    public class Settings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Key { get; set; } // e.g., "ProfitMargin"

        [Required]
        public required string Value { get; set; } // The profit margin value (e.g., 0.3 for 30%)
    }
}
