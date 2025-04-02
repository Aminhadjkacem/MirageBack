using DHP_Manager.Models.ENUM;

namespace DHP_Manager.Models.response
{
    public class QuotationResponse
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public CustomerType CustomerType { get; set; }
        public object CustomerDetails { get; set; }
        public List<DesignationResponse> Designations { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public decimal TotalPriceHT { get; set; }
        public DateTime? QuotationCreatedAt { get; set; }
        public DateTime? InvoiceCreatedAt { get; set; }
        public QuotationStatus Status { get; set; } // Added Status property
    }
}
