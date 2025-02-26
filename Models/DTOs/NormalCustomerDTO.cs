using DHP_Manager.Models.Input;

namespace DHP_Manager.Models.DTOs
{
    public class NormalCustomerDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string? Email { get; set; }
        public string CIN { get; set; }

        // Conversion method
        public NormalCustomerInput ToInput()
        {
            return new NormalCustomerInput
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
