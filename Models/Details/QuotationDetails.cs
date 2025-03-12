using DHP_Manager.Models.Entities;

namespace DHP_Manager.Models.Details
{
    public class QuotationDetails
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public string ProjectName { get; set; }
        public List<QuotationProductDetails> QuotationProducts { get; set; } = new();

        // Converts from entity to details model
        public static QuotationDetails FromEntity(Quotation entity)
        {
            return new QuotationDetails
            {
                Id = entity.Id,
                Designation = entity.Designation,
                ProjectName = entity.ProjectName,
                QuotationProducts = entity.QuotationProducts.Select(qp => QuotationProductDetails.FromEntity(qp)).ToList()
            };
        }
    }
}
