using DHP_Manager.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace DHP_Manager.Models.input
{
    public class QuotationUpdateInput
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string ProjectName { get; set; }

        public List<DesignationUpdateInput> Designations { get; set; } = new();

        public static Quotation ToEntity(QuotationUpdateInput input, Quotation existingQuotation = null)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var quotation = existingQuotation ?? new Quotation
            {
                ProjectName = input.ProjectName
            };

            quotation.Id = input.Id;
            quotation.ProjectName = input.ProjectName;

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
    }
}
