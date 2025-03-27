using DHP_Manager.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DHP_Manager.Models.Details
{
    public class DesignationDetail
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int QuotationId { get; set; }
        public string QuotationName { get; set; } = string.Empty;
        public int Quantity { get; set; }

        // ✅ List of products assigned to this designation
        public List<DesignationProductDetail> Products { get; set; } = new();

        public static DesignationDetail FromEntity(Designation entity)
        {
            return new DesignationDetail
            {
                Id = entity.Id,
                Name = entity.Name,
                QuotationId = entity.QuotationId,
                QuotationName = entity.Quotation.ProjectName, // ✅ Get Quotation name
                Quantity = entity.Quantity,
                Products = entity.DesignationProducts.Select(DesignationProductDetail.FromEntity).ToList()
            };
        }
    }
}
