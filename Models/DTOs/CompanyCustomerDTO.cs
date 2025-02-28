using DHP_Manager.Models.input;

namespace DHP_Manager.Models.DTOs
{
    public class CompanyCustomerDtO
    {
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

        // Conversion method
        public static CompanyCustomerInput toInput(CompanyCustomerDtO Dto)
        {
            return new CompanyCustomerInput
            {
                CompanyName = Dto.CompanyName,
                Address = Dto.Address,
                Ville = Dto.Ville,
                Email = Dto.Email,
                MatriculeFiscal = Dto.MatriculeFiscal,
                Contact1Name = Dto.Contact1Name,
                Contact1Phone = Dto.Contact1Phone,
                Contact2Name = Dto.Contact2Name,
                Contact2Phone = Dto.Contact2Phone,
                Contact3Name = Dto.Contact3Name,
                Contact3Phone = Dto.Contact3Phone
            };
        }
    }
}
