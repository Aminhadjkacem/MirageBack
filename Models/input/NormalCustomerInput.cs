using DHP_Manager.Models.Entities;

namespace DHP_Manager.Models.Input
{
    public class NormalCustomerInput
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string? Email { get; set; }
        public string CIN { get; set; }

        public NormalCustomer ToEntity()
        {
            return new NormalCustomer
            {
                FullName = this.FullName,
                PhoneNumber = this.PhoneNumber,
                Address = this.Address,
                Email = this.Email,
                CIN = this.CIN
            };
        }
    }
}
