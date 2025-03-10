namespace DHP_Manager.Models.response
{
    public class ProductResponse
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public required string Category { get; set; }
    }
}
