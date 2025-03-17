namespace DHP_Manager.Models.Input
{
    public class QuotationInput
    {
        public required string ProjectName { get; set; }
        public List<DesignationInput> Designations { get; set; } = new();
    }
}