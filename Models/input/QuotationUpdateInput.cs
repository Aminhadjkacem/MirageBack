using DHP_Manager.Models.Entities;
using DHP_Manager.Models.ENUM;
using System.ComponentModel.DataAnnotations;

namespace DHP_Manager.Models.input
{
    public class QuotationUpdateInput
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string ProjectName { get; set; }
        public CustomerType CustomerType { get; set; }
        public int CustomerId { get; set; }
        public List<DesignationUpdateInput> Designations { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public decimal TotalPriceHT { get; set; }
        public DateTime? QuotationCreatedAt { get; set; } // Nullable date for quotation creation
        public DateTime? InvoiceCreatedAt { get; set; } // Nullable date for invoice creation
        public static Quotation ToEntity(QuotationUpdateInput input, Quotation existingQuotation = null)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var quotation = existingQuotation ?? new Quotation
            {
                ProjectName = input.ProjectName
            };

            if (input.CustomerType == CustomerType.NORMAL)
            {

                quotation.Id = input.Id;
                quotation.ProjectName = input.ProjectName;
                quotation.CustomerType = input.CustomerType;
                quotation.NormalCustomerId = input.CustomerId;
                quotation.TotalPrice = input.TotalPrice;
                quotation.TotalPriceHT = input.TotalPriceHT;
                quotation.QuotationCreatedAt = input.QuotationCreatedAt;
                quotation.InvoiceCreatedAt = input.InvoiceCreatedAt;
                if (input.Designations != null)
                {
                    if (existingQuotation == null)
                    {
                        quotation.Designations = input.Designations
                            .Select(d => DesignationUpdateInput.ToEntity(d, input.Id))
                            .ToList();
                    }
                }

                return quotation;
            }
            else
            {

                quotation.Id = input.Id;
                quotation.ProjectName = input.ProjectName;
                quotation.CustomerType = input.CustomerType;
                quotation.CompanyCustomerId = input.CustomerId;
                quotation.TotalPrice = input.TotalPrice;
                quotation.TotalPriceHT = input.TotalPriceHT;
                if (input.Designations != null)
                {
                    if (existingQuotation == null)
                    {
                        quotation.Designations = input.Designations
                            .Select(d => DesignationUpdateInput.ToEntity(d, input.Id))
                            .ToList();
                    }
                }

                return quotation;}
        }
    }
}
