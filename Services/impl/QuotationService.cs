using DHP_Manager.Models.DTOs;
using DHP_Manager.Models.Details;
using DHP_Manager.Models.Entities;
using DHP_Manager.Models.input;
using DHP_Manager.Models.Input;
using DHP_Manager.Repositories.Interfaces;
using DHP_Manager.Services.Interfaces;

namespace DHP_Manager.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly IQuotationRepository _quotationRepository;

        public QuotationService(IQuotationRepository quotationRepository)
        {
            _quotationRepository = quotationRepository;
        }

        public async Task<IEnumerable<QuotationDetails>> GetAllQuotationsAsync()
        {
            var quotations = await _quotationRepository.GetAllAsync();
            return quotations.Select(q => QuotationDetails.FromEntity(q)).ToList();
        }

        public async Task<QuotationDetails?> GetQuotationByIdAsync(int id)
        {
            var quotation = await _quotationRepository.GetByIdAsync(id);
            return quotation == null ? null : QuotationDetails.FromEntity(quotation);
        }

        public async Task<QuotationDetails> CreateQuotationAsync(QuotationInput quotationInput)
        {
            var quotationEntity = QuotationInput.ToEntity(quotationInput); // Mapping from input to entity
            var createdQuotation = await _quotationRepository.AddAsync(quotationEntity);

            return QuotationDetails.FromEntity(createdQuotation); // Mapping from entity to details
        }

        public async Task<QuotationDetails> UpdateQuotationAsync(int id, QuotationInput quotationInput)
        {
            var existingQuotation = await _quotationRepository.GetByIdAsync(id);
            if (existingQuotation == null)
            {
                throw new ArgumentException("Quotation not found.");
            }

            // Update fields of the existing entity based on the input
            existingQuotation.Designation = quotationInput.Designation;
            existingQuotation.ProjectName = quotationInput.ProjectName;
            existingQuotation.QuotationProducts = quotationInput.QuotationProducts.Select(qp => QuotationProductInput.ToEntity(qp)).ToList();

            var updatedQuotation = await _quotationRepository.UpdateAsync(existingQuotation);

            return QuotationDetails.FromEntity(updatedQuotation); // Mapping from entity to details
        }

        public async Task<bool> DeleteQuotationAsync(int id)
        {
            return await _quotationRepository.DeleteAsync(id);
        }
    }
}
