using DHP_Manager.Models.Entities;

namespace DHP_Manager.Models.Details
{
    public class QuotationProductDetails
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // Converts from entity to product details model
        public static QuotationProductDetails FromEntity(QuotationProduct entity)
        {
            return new QuotationProductDetails
            {
                ProductId = entity.ProductId,
                Quantity = entity.Quantity
            };
        }
    }
}
