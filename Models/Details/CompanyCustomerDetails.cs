using DHP_Manager.Models.Entities;

namespace DHP_Manager.Models.Details
{
    public class CompanyCustomerDetails
    {
        public int id { get; set; }
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
        public static CompanyCustomerDetails fromEntity(CompanyCustomer entity)
        {
            if (entity == null) return null;

            return new CompanyCustomerDetails
            {
                id = entity.Id,
                CompanyName = entity.CompanyName,
                Address = entity.Address,
                Ville = entity.Ville,
                Email = entity.Email,
                MatriculeFiscal = entity.MatriculeFiscal,
                Contact1Name = entity.Contact1Name,
                Contact1Phone = entity.Contact1Phone,
                Contact2Name = entity.Contact2Name,
                Contact2Phone = entity.Contact2Phone,
                Contact3Name = entity.Contact3Name,
                Contact3Phone = entity.Contact3Phone
            };
        }
    }
}
