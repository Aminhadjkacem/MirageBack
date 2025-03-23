using DHP_Manager.Helpers.Exceptions;
using DHP_Manager.Models.Details;
using DHP_Manager.Models.Entities;
using DHP_Manager.Models.ENUM;
using DHP_Manager.Models.input;
using DHP_Manager.Models.Input;
using DHP_Manager.Models.response;
using DHP_Manager.Repositories.Impl;
using DHP_Manager.Repositories.Interface;
using DHP_Manager.Services.Impl;
using DHP_Manager.Services.Interface;

namespace DHP_Manager.Services.impl
{
    public class QuotationService : IQuotationService
    {
        private readonly IQuotationRepository _quotationRepository;
        private readonly IDesignationRepository _designationRepository;
        private readonly IProductService _productService;
        private readonly ICompanyCustomerService _companyCustomerService;
        private readonly INormalCustomerService _normalCustomerService;
        private readonly ISettingsService _settingsService;

        public QuotationService(
            IQuotationRepository quotationRepository,
            IDesignationRepository designationRepository,
            IProductService productService,
            ICompanyCustomerService companyCustomerService,
            INormalCustomerService normalCustomerService,
            ISettingsService settingsService)
        {
            _quotationRepository = quotationRepository;
            _designationRepository = designationRepository;
            _productService = productService;
            _companyCustomerService = companyCustomerService;
            _normalCustomerService = normalCustomerService;
            _settingsService = settingsService;
        }

        public async Task<QuotationResponse> CreateQuotationAsync(QuotationInput quotationInput)
        {
            if (quotationInput == null)
                throw new ArgumentNullException(nameof(quotationInput), "Quotation input cannot be null.");

            if (string.IsNullOrWhiteSpace(quotationInput.ProjectName))
                throw new ArgumentException("Project name cannot be empty", nameof(quotationInput.ProjectName));

            // Check stock availability
            if (!await _productService.VerifyStockAsync(quotationInput.Designations.SelectMany(d => d.Products).ToList()))
                throw new InsufficientStockException("Not enough stock for one or more products.");

            // Validate customer exists based on CustomerType
            if (quotationInput.CustomerType == CustomerType.NORMAL)
            {
                var normalCustomer = await _normalCustomerService.GetByIdAsync(quotationInput.CustomerId);
                if (normalCustomer == null)
                    throw new KeyNotFoundException($"Normal customer with ID {quotationInput.CustomerId} not found.");
            }
            else if (quotationInput.CustomerType == CustomerType.COMPANY)
            {
                var companyCustomer = await _companyCustomerService.GetByIdAsync(quotationInput.CustomerId);
                if (companyCustomer == null)
                    throw new KeyNotFoundException($"Company customer with ID {quotationInput.CustomerId} not found.");
            }
            else
            {
                throw new ArgumentException("Invalid CustomerType specified.", nameof(quotationInput.CustomerType));
            }

            // Create the quotation entity
            var quotation = new Quotation
            {
                ProjectName = quotationInput.ProjectName,
                CustomerType = quotationInput.CustomerType,
                NormalCustomerId = quotationInput.CustomerType == CustomerType.NORMAL ? quotationInput.CustomerId : null,
                CompanyCustomerId = quotationInput.CustomerType == CustomerType.COMPANY ? quotationInput.CustomerId : null,
                Designations = new List<Designation>(),
                Status = QuotationStatus.PENDING
            };

            // Calculate base price and build designations
            decimal basePrice = 0m;
            foreach (var designationInput in quotationInput.Designations)
            {
                var designation = new Designation
                {
                    Name = designationInput.Name,
                    DesignationProducts = new List<DesignationProduct>()
                };

                // Group products by ProductId and sum quantities
                var productGroups = designationInput.Products
                    .GroupBy(p => p.ProductId)
                    .Select(g => new
                    {
                        ProductId = g.Key,
                        TotalQuantity = g.Sum(p => p.Quantity)
                    });

                foreach (var productGroup in productGroups)
                {
                    var product = await _productService.GetProductByIdAsync(productGroup.ProductId);
                    if (product == null)
                        throw new KeyNotFoundException($"Product with ID {productGroup.ProductId} not found.");

                    designation.DesignationProducts.Add(new DesignationProduct
                    {
                        ProductId = productGroup.ProductId,
                        Quantity = productGroup.TotalQuantity
                    });

                    basePrice += product.Price * productGroup.TotalQuantity;
                }

                quotation.Designations.Add(designation);
            }

            // Fetch settings for price calculation
            decimal profitMargin = await _settingsService.GetProfitMarginAsync();
            decimal tvaMargin = await _settingsService.GetTVAMarginAsync();
            decimal droitDuTimbrePrice = await _settingsService.GetDroitDuTimbrePriceAsync();

            // Calculate total price
            decimal priceWithProfit = basePrice * (1 + profitMargin);
            decimal priceWithTVA = priceWithProfit * (1 + tvaMargin);
            decimal totalPrice = priceWithTVA + droitDuTimbrePrice;

            // Set the calculated total price on the quotation
            quotation.TotalPrice = totalPrice;
            quotation.TotalPriceHT = priceWithProfit;

            // Save the quotation and its designations in one go
            await _quotationRepository.AddAsync(quotation);

            var createdQuotation = await _quotationRepository.GetQuotationWithDesignationsAsync(quotation.Id);
            return MapToResponse(createdQuotation);
        }
        public async Task<Quotation> GetQuotationAsync(int id)
        {
            return await _quotationRepository.GetByIdAsync(id);
        }

        public async Task<List<QuotationResponse>> GetValidatedQuotationsAsync()
        {
            var validatedQuotations = await _quotationRepository.GetValidatedQuotationsAsync();
            return validatedQuotations.Select(MapToResponse).ToList();
        }

        public async Task ValidateQuotationAsync(int id)
        {
            using var transaction = await _quotationRepository.BeginTransactionAsync();
            try
            {
                var quotation = await _quotationRepository.GetQuotationWithDesignationsAsync(id);
                if (quotation == null)
                    throw new KeyNotFoundException($"Quotation with ID {id} not found.");

                if (quotation.Status == QuotationStatus.VALIDATED)
                    throw new InvalidOperationException($"Quotation with ID {id} is already validated.");

                var productQuantities = quotation.Designations
                    .SelectMany(d => d.DesignationProducts)
                    .Select(dp => new ProductQuantityInput
                    {
                        ProductId = dp.ProductId,
                        Quantity = dp.Quantity
                    })
                    .ToList();

                await _productService.VerifyAndUpdateStockAsync(productQuantities);
                await _quotationRepository.UpdateQuotationStatusAsync(id, QuotationStatus.VALIDATED);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<QuotationResponse>> GetAllQuotationsAsync()
        {
            var quotations = await _quotationRepository.GetAllAsync();
            return quotations.Select(MapToResponse);
        }

        public async Task<QuotationResponse> GetQuotationWithDesignationsAsync(int id)
        {
            var quotation = await _quotationRepository.GetQuotationWithDesignationsAsync(id);
            return MapToResponse(quotation);
        }

        public async Task UpdateQuotationAsync(QuotationUpdateInput quotationInput)
        {
            if (quotationInput == null)
                throw new ArgumentNullException(nameof(quotationInput));

            var quotationEntity = QuotationUpdateInput.ToEntity(quotationInput);
            await _quotationRepository.UpdateAsync(quotationEntity);
        }

        public async Task DeleteQuotationAsync(int id)
        {
            await _quotationRepository.DeleteAsync(id);
        }

        public async Task AddDesignationToQuotationAsync(int quotationId, string designationName)
        {
            if (string.IsNullOrWhiteSpace(designationName))
                throw new ArgumentException("Designation name cannot be empty", nameof(designationName));

            var quotation = await _quotationRepository.GetByIdAsync(quotationId);
            var designation = new Designation
            {
                Name = designationName,
                QuotationId = quotationId,
                DesignationProducts = new List<DesignationProduct>()
            };

            await _designationRepository.AddAsync(designation);
        }

        private QuotationResponse MapToResponse(Quotation quotation)
        {
            if (quotation == null)
                throw new ArgumentNullException(nameof(quotation));

            var response = new QuotationResponse
            {
                Id = quotation.Id,
                ProjectName = quotation.ProjectName,
                CreatedAt = quotation.CreatedAt,
                CustomerType = quotation.CustomerType,
                Designations = quotation.Designations.Select(d => new DesignationResponse
                {
                    Id = d.Id,
                    Name = d.Name,
                    Products = d.DesignationProducts.Select(dp => new ProductQuantityResponse
                    {
                        ProductId = dp.ProductId,
                        ProductName = dp.Product?.Name ?? "Unknown",
                        Quantity = dp.Quantity
                    }).ToList()
                }).ToList(),
                TotalPrice = quotation.TotalPrice, // Use persisted value
                TotalPriceHT = quotation.TotalPriceHT, // Use persisted value
                Status = quotation.Status
            };

            response.CustomerDetails = quotation.CustomerType switch
            {
                CustomerType.NORMAL => NormalCustomerDetails.fromEntity(quotation.NormalCustomer),
                CustomerType.COMPANY => CompanyCustomerDetails.fromEntity(quotation.CompanyCustomer),
                _ => throw new InvalidOperationException("Unknown CustomerType")
            };

            return response;
        }
    }
}
