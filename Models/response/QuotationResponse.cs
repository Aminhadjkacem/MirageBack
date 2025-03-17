namespace DHP_Manager.Models.response
{
    public class QuotationResponse
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<DesignationResponse> Designations { get; set; } = new();
    }
}
