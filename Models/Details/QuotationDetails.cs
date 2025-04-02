using DHP_Manager.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DHP_Manager.Models.Details
{
    public class QuotationDetail
    {
        public int Id { get; set; }
        public required string ProjectName { get; set; }

        // ✅ List of designations in this quotation
        public List<DesignationDetail> Designations { get; set; } = new();
        public DateTime? QuotationCreatedAt { get; set; }
        public DateTime? InvoiceCreatedAt { get; set; }
        public decimal TotalPrice { get; set; }

        public static QuotationDetail FromEntity(Quotation entity)
        {
            return new QuotationDetail
            {
                Id = entity.Id,
                ProjectName = entity.ProjectName,
                QuotationCreatedAt = entity.QuotationCreatedAt,
                InvoiceCreatedAt = entity.InvoiceCreatedAt,
                Designations = entity.Designations.Select(DesignationDetail.FromEntity).ToList()
            };
        }
    }
}
