using DHP_Manager.Models.Details;

namespace DHP_Manager.Models.response
{
    public class CompanyCustomerResponse
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

        public static CompanyCustomerResponse fromDetails(CompanyCustomerDetails details)
        {
            return new CompanyCustomerResponse
            {
                CompanyName = details.CompanyName,
                Address = details.Address,
                Ville = details.Ville,
                Email = details.Email,
                MatriculeFiscal = details.MatriculeFiscal,
                Contact1Name = details.Contact1Name,
                Contact1Phone = details.Contact1Phone,
                Contact2Name = details.Contact2Name,
                Contact2Phone = details.Contact2Phone,
                Contact3Name = details.Contact3Name,
                Contact3Phone = details.Contact3Phone
            };
        }
    }
}
