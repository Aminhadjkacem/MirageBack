namespace DHP_Manager.Models.response
{
    public class DesignationResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public List<ProductQuantityResponse> Products { get; set; } = new();
    }
}
