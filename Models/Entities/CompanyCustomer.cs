using System.ComponentModel.DataAnnotations;

namespace DHP_Manager.Models.Entities
{
    public class CompanyCustomer
    {
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string Ville { get; set; }

        public string Email { get; set; }

        [Required]
        public string MatriculeFiscal { get; set; }
        
        [Required]
        public string Contact1Name { get; set; }
        [Required]

        public string Contact1Phone { get; set; }

        public string Contact2Name { get; set; }
        public string Contact2Phone { get; set; }

        public string Contact3Name { get; set; }
        public string Contact3Phone { get; set; }
    }

}
