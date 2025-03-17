using DHP_Manager.Models.Entities;
using DHP_Manager.Repositories.Interface;
using DHP_Manager.Services.Interface;

namespace DHP_Manager.Services.impl
{
    public class DesignationService : IDesignationService
    {
        private readonly IDesignationRepository _designationRepository;

        public DesignationService(IDesignationRepository designationRepository)
        {
            _designationRepository = designationRepository;
        }

        public async Task<Designation> CreateDesignationAsync(int quotationId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Designation name cannot be empty", nameof(name));

            var designation = new Designation
            {
                Name = name,
                QuotationId = quotationId,
                DesignationProducts = new List<DesignationProduct>()
            };

            await _designationRepository.AddAsync(designation);
            return designation;
        }

        public async Task<Designation> GetDesignationAsync(int id)
        {
            return await _designationRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Designation>> GetDesignationsByQuotationAsync(int quotationId)
        {
            return await _designationRepository.GetAllByQuotationIdAsync(quotationId);
        }

        public async Task<Designation> GetDesignationWithProductsAsync(int id)
        {
            return await _designationRepository.GetDesignationWithProductsAsync(id);
        }

        public async Task UpdateDesignationAsync(Designation designation)
        {
            if (designation == null)
                throw new ArgumentNullException(nameof(designation));

            await _designationRepository.UpdateAsync(designation);
        }

        public async Task DeleteDesignationAsync(int id)
        {
            await _designationRepository.DeleteAsync(id);
        }

        public async Task AddProductToDesignationAsync(int designationId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            var designation = await _designationRepository.GetDesignationWithProductsAsync(designationId);

            var designationProduct = new DesignationProduct
            {
                DesignationId = designationId,
                ProductId = productId,
                Quantity = quantity
            };

            designation.DesignationProducts.Add(designationProduct);
            await _designationRepository.UpdateAsync(designation);
        }
    }
}
