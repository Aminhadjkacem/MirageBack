using DHP_Manager.Models.Entities;

namespace DHP_Manager.Models.input
{
    public class QuotationProductInput
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // Converts input to the QuotationProduct entity
        public static QuotationProduct ToEntity(QuotationProductInput input)
        {
            return new QuotationProduct
            {
                ProductId = input.ProductId,
                Quantity = input.Quantity
            };
        }
    }
}
