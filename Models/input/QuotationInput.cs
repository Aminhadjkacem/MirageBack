using DHP_Manager.Models.ENUM;

namespace DHP_Manager.Models.Input
{
    public class QuotationInput
    {
        public required string ProjectName { get; set; }
        public CustomerType CustomerType { get; set; }
        public int CustomerId { get; set; }
        public List<DesignationInput> Designations { get; set; } = new();
    }
}