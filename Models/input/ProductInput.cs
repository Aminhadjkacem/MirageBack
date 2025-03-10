using DHP_Manager.Models.Entities;

namespace DHP_Manager.Models.input
{
    public class ProductInput
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public required string Category { get; set; }

        // Mapping function to convert ProductInput to Product (Entity)
        public Product ToEntity(ProductInput input)
        {
            return new Product
            {
                Name = input.Name,
                Description = input.Description,
                Price = input.Price,
                Stock = input.Stock,
                Category = input.Category
            };
        }
    }
}
