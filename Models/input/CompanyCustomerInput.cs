using DHP_Manager.Models.Entities;

namespace DHP_Manager.Models.input
{
    public class CompanyCustomerInput
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Ville { get; set; }
        public string Email { get; set; }
        public string MatriculeFiscal { get; set; }
        public string Contact1Name { get; set; }
        public string Contact1Phone { get; set; }
        public string Contact2Name { get; set; }
        public string Contact2Phone { get; set; }
        public string Contact3Name { get; set; }
        public string Contact3Phone { get; set; }
        public CompanyCustomer ToEntity()
        {
            return new CompanyCustomer
            {
                Id = this.Id,
                CompanyName = this.CompanyName,
                Address = this.Address,
                Ville = this.Ville,
                Email = this.Email,
                MatriculeFiscal = this.MatriculeFiscal,
                Contact1Name = this.Contact1Name,
                Contact1Phone = this.Contact1Phone,
                Contact2Name = this.Contact2Name,
                Contact2Phone = this.Contact2Phone,
                Contact3Name = this.Contact3Name,
                Contact3Phone = this.Contact3Phone
            };
        }
        }
}
