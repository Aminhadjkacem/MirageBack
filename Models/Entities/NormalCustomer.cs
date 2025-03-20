using System.ComponentModel.DataAnnotations;

namespace DHP_Manager.Models.Entities
{
    public class NormalCustomer
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string? Email { get; set; }

        [Required]
        public string CIN { get; set; }

        // One NormalCustomer -> Many Quotations
        public List<Quotation> Quotations { get; set; } = new();
    }
}
