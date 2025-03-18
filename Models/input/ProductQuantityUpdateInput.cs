using DHP_Manager.Models.Entities;

namespace DHP_Manager.Models.input
{
    public class ProductQuantityUpdateInput
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public static DesignationProduct ToEntity(ProductQuantityUpdateInput input, DesignationProduct existingProduct = null)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var designationProduct = existingProduct ?? new DesignationProduct();
            designationProduct.ProductId = input.ProductId;
            designationProduct.Quantity = input.Quantity;

            return designationProduct;
        }
    }
}

