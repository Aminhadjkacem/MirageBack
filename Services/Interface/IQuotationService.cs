using DHP_Manager.Models.Entities;
using DHP_Manager.Models.input;
using DHP_Manager.Models.Input;
using DHP_Manager.Models.response;

namespace DHP_Manager.Services.Interface
{
    public interface IQuotationService
    {
        Task<QuotationResponse> CreateQuotationAsync(QuotationInput quotationInput); // Updated return type
        Task<Quotation> GetQuotationAsync(int id); // Keep as is for internal use
        Task<IEnumerable<QuotationResponse>> GetAllQuotationsAsync(); // Updated return type
        Task<QuotationResponse> GetQuotationWithDesignationsAsync(int id); // Updated return type
        Task UpdateQuotationAsync(QuotationUpdateInput quotation);
        Task DeleteQuotationAsync(int id);
        Task AddDesignationToQuotationAsync(int quotationId, string designationName);
        Task ValidateQuotationAsync(int id);
        Task<List<QuotationResponse>> GetValidatedQuotationsAsync();
    }
}
