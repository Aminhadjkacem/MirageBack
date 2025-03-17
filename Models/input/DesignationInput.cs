namespace DHP_Manager.Models.Input
{
    public class DesignationInput
    {
        public required string Name { get; set; }
        public List<ProductQuantityInput> Products { get; set; } = new();
    }
}