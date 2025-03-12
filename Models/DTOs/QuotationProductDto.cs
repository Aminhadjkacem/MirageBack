using DHP_Manager.Models.input;

namespace DHP_Manager.Models.DTOs
{
    public class QuotationProductDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // Converts DTO to QuotationProductInput
        public static QuotationProductInput ToInput(QuotationProductDto dto)
        {
            return new QuotationProductInput
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };
        }
    }
}
