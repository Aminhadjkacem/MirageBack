using DHP_Manager.Models.ENUM;

namespace DHP_Manager.Models.response
{
    public class QuotationResponse
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreatedAt { get; set; }
        public CustomerType CustomerType { get; set; } // To indicate the type of customer
        public object CustomerDetails { get; set; } // Can hold NormalCustomerDetails or CompanyCustomerDetails
        public List<DesignationResponse> Designations { get; set; } = new();
    }
}
