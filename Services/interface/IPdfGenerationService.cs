using DHP_Manager.Models.input;

namespace DHP_Manager.Services.Interface
{
    public interface IPdfGenerationService
    {
        byte[] GenerateQuotationPdf(QuotationUpdateInput quotation);
    }
}
