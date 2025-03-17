namespace DHP_Manager.Models.Input
{
    public class QuotationDesignationInput
    {
        public int QuotationId { get; set; } // Link to the Quotation entity

        // List of Designation Inputs for the quotation
        public List<DesignationInput> Designations { get; set; } = new();
    }
}
