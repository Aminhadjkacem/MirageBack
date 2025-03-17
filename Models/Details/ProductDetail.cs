namespace DHP_Manager.Models.Details
{
    public class ProductDetail
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public required string Category { get; set; }
        public List<string> Designations { get; set; } = new(); // Added to store designation names
    }
}