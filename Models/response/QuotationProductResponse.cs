using DHP_Manager.Models.Details;

namespace DHP_Manager.Models.response
{
    public class QuotationProductResponse
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // Converts from product details to response model
        public static QuotationProductResponse FromDetails(QuotationProductDetails details)
        {
            return new QuotationProductResponse
            {
                ProductId = details.ProductId,
                Quantity = details.Quantity
            };
        }
    }
}
