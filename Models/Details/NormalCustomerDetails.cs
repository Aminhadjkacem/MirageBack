using DHP_Manager.Models.Entities;

namespace DHP_Manager.Models.Details
{
    public class NormalCustomerDetails
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string? Email { get; set; }
        public string CIN { get; set; }

        // Conversion method
        public static NormalCustomerDetails fromEntity(NormalCustomer entity)
        {
            if (entity == null) return null;

            return new NormalCustomerDetails
            {
                FullName = entity.FullName,
                PhoneNumber = entity.PhoneNumber,
                Address = entity.Address,
                Email = entity.Email,
                CIN = entity.CIN
            };
        }
    }

}
