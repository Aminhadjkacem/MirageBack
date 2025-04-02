using DHP_Manager.Models.input;

namespace DHP_Manager.Services.Interface
{
    public interface IDevisPDFGeneratorService
    {
        byte[] GenerateQuotationPdf(QuotationUpdateInput quotation);
    }
}
