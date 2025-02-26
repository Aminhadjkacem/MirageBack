using DHP_Manager.Models.Details;

namespace DHP_Manager.Models.response

{
    public class NormalCustomerResponse
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string? Email { get; set; }
        public string CIN { get; set; }

        public static NormalCustomerResponse fromDetails(NormalCustomerDetails details)
        {
            if (details == null) return null;

            return new NormalCustomerResponse
            {
                FullName = details.FullName,
                PhoneNumber = details.PhoneNumber,
                Address = details.Address,
                Email = details.Email,
                CIN = details.CIN
            };
        }
    }

    

}
