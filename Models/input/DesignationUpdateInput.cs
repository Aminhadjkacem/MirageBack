using DHP_Manager.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace DHP_Manager.Models.input
{
    public class DesignationUpdateInput
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public List<ProductQuantityUpdateInput> Products { get; set; } = new();

        public static Designation ToEntity(DesignationUpdateInput input, int quotationId, Designation existingDesignation = null)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var designation = existingDesignation ?? new Designation
            {
                Name = input.Name,
                QuotationId = quotationId // Set the foreign key
            };

            designation.Id = input.Id;
            designation.Name = input.Name;
            designation.QuotationId = quotationId; // Ensure it’s set even for existing designations

            if (input.Products != null && existingDesignation == null)
            {
                designation.DesignationProducts = input.Products
                    .Select(p => ProductQuantityUpdateInput.ToEntity(p))
                    .ToList();
            }

            return designation;
        }
    }
}
