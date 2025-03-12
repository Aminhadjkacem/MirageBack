using DHP_Manager.Models.Entities;

namespace DHP_Manager.Models.input
{
    public class QuotationInput
    {
        public required string Designation { get; set; }
        public required string ProjectName { get; set; }
        public List<QuotationProductInput> QuotationProducts { get; set; } = new();

        // Converts input to the entity model
        public static Quotation ToEntity(QuotationInput input)
        {
            return new Quotation
            {
                Designation = input.Designation,
                ProjectName = input.ProjectName,
                QuotationProducts = input.QuotationProducts.Select(qp => QuotationProductInput.ToEntity(qp)).ToList()
            };
        }
    }
}
