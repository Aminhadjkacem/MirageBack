using DHP_Manager.Models.input;

namespace DHP_Manager.Models.DTOs
{
    public class ProductDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public required string Category { get; set; }

        // Mapping function to convert ProductDto to ProductInput
        public static ProductInput ToInput(ProductDto dto)
        {
            return new ProductInput
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                Category = dto.Category
            };
        }
    }
}
