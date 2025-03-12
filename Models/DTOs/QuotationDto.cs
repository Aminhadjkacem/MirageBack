using DHP_Manager.Models.input;

namespace DHP_Manager.Models.DTOs
{
    public class QuotationDto
    {
        public required string Designation { get; set; }
        public required string ProjectName { get; set; }
        public List<QuotationProductDto> QuotationProducts { get; set; } = new();

        // Converts DTO to input model
        public static QuotationInput ToInput(QuotationDto dto)
        {
            return new QuotationInput
            {
                Designation = dto.Designation,
                ProjectName = dto.ProjectName,
                QuotationProducts = dto.QuotationProducts.Select(qp => QuotationProductDto.ToInput(qp)).ToList()
            };
        }
    }
}
