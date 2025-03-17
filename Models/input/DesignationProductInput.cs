using DHP_Manager.Models.Entities;

namespace DHP_Manager.Models.Input
{
    public class DesignationProductInput
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // Convert to Entity
        public static DesignationProduct ToEntity(DesignationProductInput input)
        {
            return new DesignationProduct
            {
                ProductId = input.ProductId,
                Quantity = input.Quantity
            };
        }
    }
}
