using DHP_Manager.Models.Details;
using DHP_Manager.Models.input;

namespace DHP_Manager.Services.Interfaces
{
    public interface IQuotationService
    {
        Task<IEnumerable<QuotationDetails>> GetAllQuotationsAsync();
        Task<QuotationDetails?> GetQuotationByIdAsync(int id);
        Task<QuotationDetails> CreateQuotationAsync(QuotationInput quotationInput);
        Task<QuotationDetails> UpdateQuotationAsync(int id, QuotationInput quotationInput);
        Task<bool> DeleteQuotationAsync(int id);
    }
}
