using DHP_Manager.Models.Entities;

namespace DHP_Manager.Models.Details
{
    public class DesignationProductDetail
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public static DesignationProductDetail FromEntity(DesignationProduct entity)
        {
            return new DesignationProductDetail
            {
                ProductId = entity.ProductId,
                ProductName = entity.Product.Name, // ✅ Retrieve product name
                Quantity = entity.Quantity
            };
        }
    }
}
