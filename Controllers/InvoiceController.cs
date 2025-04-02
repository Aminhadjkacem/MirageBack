using Microsoft.AspNetCore.Mvc;
using DHP_Manager.Models.input;
using DHP_Manager.Services;
using DHP_Manager.Services.Interface;

namespace DHP_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IPdfGenerationService _pdfService;
        private readonly IDevisPDFGeneratorService _devisPDFGeneratorService;

        public InvoiceController(IPdfGenerationService pdfService, IDevisPDFGeneratorService devisPDFGeneratorService)
        {
            _pdfService = pdfService;
            _devisPDFGeneratorService = devisPDFGeneratorService;
        }

        [HttpPost("export-quotation")]
        public IActionResult ExportQuotation([FromBody] QuotationUpdateInput quotation)
        {
            if (quotation == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid Invoice data.");
            }

            var pdfBytes = _pdfService.GenerateQuotationPdf(quotation);
            return File(pdfBytes, "application/pdf", $"Invoice_{quotation.Id}.pdf");
        }
        [HttpPost("export-Devis")]
        public IActionResult ExportDevis([FromBody] QuotationUpdateInput quotation)
        {
            if (quotation == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid quotation data.");
            }

            var pdfBytes = _devisPDFGeneratorService.GenerateQuotationPdf(quotation);
            return File(pdfBytes, "application/pdf", $"quotation_{quotation.Id}.pdf");
        }
    }
}