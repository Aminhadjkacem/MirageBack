using DHP_Manager.Models.Details;

namespace DHP_Manager.Models.response
{
    public class QuotationResponse
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public string ProjectName { get; set; }
        public List<QuotationProductResponse> QuotationProducts { get; set; }

        // Converts from details to response model
        public static QuotationResponse FromDetails(QuotationDetails details)
        {
            return new QuotationResponse
            {
                Id = details.Id,
                Designation = details.Designation,
                ProjectName = details.ProjectName,
                QuotationProducts = details.QuotationProducts.Select(qp => QuotationProductResponse.FromDetails(qp)).ToList()
            };
        }
    }
}
