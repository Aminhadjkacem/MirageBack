using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;
using DHP_Manager.Models.input;
using DHP_Manager.Services.Interface;

namespace DHP_Manager.Services.impl
{
    public class PdfGenerationService : IPdfGenerationService
    {
        private readonly ISettingsService _settingsService;
        private readonly IQuotationService _quotationService;

        public PdfGenerationService(ISettingsService settingsService, IQuotationService quotationService)
        {
            _settingsService = settingsService;
            _quotationService = quotationService;
        }
        public byte[] GenerateQuotationPdf(QuotationUpdateInput quotation)
        {
            using var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            // Define fonts and colors
            var font = new XFont("Arial", 12);
            var smallFont = new XFont("Arial", 10);
            var blueBrush = XBrushes.DarkBlue;
            var orangeBrush = XBrushes.Orange;
            var blackBrush = XBrushes.Black;

            // Header: Draw logo (assuming logo.png is in the specified path)
            string logoPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "Services", "impl", "logo.png");
            if (File.Exists(logoPath))
            {
                var logo = XImage.FromFile(logoPath);
                if (logo != null && logo.Height > 0) // Vérification avant le calcul
                {
                    int newHeight = 40; // Hauteur fixe
                    int newWidth = (int)((double)(logo.Width) * newHeight / logo.Height); // Largeur ajustée proportionnellement
                    gfx.DrawImage(logo, 50, 20, newWidth, newHeight);
                }
            }

            int currentYear = DateTime.Now.Year;
            int? factureId = _quotationService.GetFactureIdAsync(quotation.Id).Result;
            // Create the text for invoice number and quotation ID
            string invoiceNumberText = $"Facture";
            string quotationIdText = $"N°: 00{factureId}/{currentYear}"; // Quotation ID text

            XFont fontInvoiceNumber = new XFont("Arial", 20); // Larger font for "Facture N°"
            XFont fontQuotationId = new XFont("Arial", 10); // Larger font for the quotation ID
            var textWidthInvoice = gfx.MeasureString(invoiceNumberText, fontInvoiceNumber).Width;
            var textWidthQuotationId = gfx.MeasureString(quotationIdText, fontQuotationId).Width;

            gfx.DrawString(invoiceNumberText, fontInvoiceNumber, orangeBrush, new XRect(page.Width - textWidthInvoice - 50, 30, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(quotationIdText, fontQuotationId, blackBrush, new XRect(page.Width - textWidthQuotationId - 50, 30 + fontInvoiceNumber.Height + 5, page.Width, page.Height), XStringFormats.TopLeft);

            // Header: Draw the horizontal line
            var penBlue = new XPen(XColors.DarkBlue, 1);
            var penOrange = new XPen(XColors.Orange, 1);
            gfx.DrawLine(penBlue, 50, 70, page.Width / 2, 70);
            gfx.DrawLine(penOrange, page.Width / 2, 70, page.Width - 50, 70);

            // Table: Define positions and dimensions
            double tableStartY = 100;
            double tableWidth = page.Width - 100; // 50 margin on each side
            double col1Width = tableWidth * 0.3; // Désignation
            double col2Width = tableWidth * 0.1; // Qté
            double col3Width = tableWidth * 0.2; // P.U.H.T
            double col4Width = tableWidth * 0.2; // Montant H.T
            double col5Width = tableWidth * 0.1; // TVA %
            double rowHeight = 20;
            double currentY = tableStartY;

            // Table Header (with the new TVA % column)
            gfx.DrawRectangle(new XSolidBrush(XColors.Orange), 50, currentY, tableWidth, rowHeight);
            gfx.DrawString("Désignation", font, blackBrush, new XRect(50, currentY, col1Width, rowHeight), XStringFormats.Center);
            gfx.DrawString("Qté", font, blackBrush, new XRect(50 + col1Width, currentY, col2Width, rowHeight), XStringFormats.Center);
            gfx.DrawString("P.U.H.T", font, blackBrush, new XRect(50 + col1Width + col2Width, currentY, col3Width, rowHeight), XStringFormats.Center);
            gfx.DrawString("Montant H.T", font, blackBrush, new XRect(50 + col1Width + col2Width + col3Width, currentY, col4Width, rowHeight), XStringFormats.Center);
            gfx.DrawString("TVA %", font, blackBrush, new XRect(50 + col1Width + col2Width + col3Width + col4Width, currentY, col5Width, rowHeight), XStringFormats.Center);
            currentY += rowHeight;

            string tvaPercentage = (_settingsService.GetTVAMarginAsync().Result * 100).ToString() + "%"; // Get the TVA margin from the settings

            // Calculate the total quantity across all designations to determine the unit price
            int totalQuantityAcrossAll = quotation.Designations?.Sum(d => d.Products?.Sum(p => p.Quantity) ?? 0) ?? 0;
            decimal averageUnitPrice = totalQuantityAcrossAll > 0 ? quotation.TotalPriceHT / totalQuantityAcrossAll : 0m;

            // Table Data
            if (quotation.Designations != null && quotation.Designations.Any())
            {
                foreach (var designation in quotation.Designations)
                {
                    // Ensure designation is not null
                    if (designation == null || string.IsNullOrEmpty(designation.Name))
                    {
                        continue; // Skip invalid designations
                    }

                    // Calculate total quantity for the designation
                    int totalQuantity = designation.Products?.Sum(p => p.Quantity) ?? 0;

                    // Calculate total HT for the designation using the average unit price
                    double totalHT = (double)(averageUnitPrice * totalQuantity);

                    // Use the average unit price for P.U.H.T
                    string unitPriceText = averageUnitPrice.ToString("F1");

                    // Adjusted column positions (shifted slightly to the right)
                    double offsetX = 18; // Small shift to the right

                    // Calculate TVA % for each row (just showing the global percentage for this example)
                    string tvaText = tvaPercentage;

                    // Draw the row data with an adjusted X position
                    gfx.DrawString(designation.Name, smallFont, blackBrush, new XPoint(50 + offsetX, currentY + rowHeight / 2), XStringFormats.CenterLeft);
                    gfx.DrawString(totalQuantity.ToString(), smallFont, blackBrush, new XPoint(50 + col1Width + offsetX, currentY + rowHeight / 2), XStringFormats.Center);
                    gfx.DrawString(unitPriceText, smallFont, blackBrush, new XPoint(50 + col1Width + col2Width + offsetX, currentY + rowHeight / 2), XStringFormats.Center);
                    gfx.DrawString(totalHT.ToString("F1"), smallFont, blackBrush, new XPoint(50 + col1Width + col2Width + col3Width + offsetX, currentY + rowHeight / 2), XStringFormats.Center);
                    gfx.DrawString(tvaText, smallFont, blackBrush, new XPoint(50 + col1Width + col2Width + col3Width + col4Width + offsetX, currentY + rowHeight / 2), XStringFormats.Center);
                    currentY += rowHeight;
                }
            }

            // Draw table borders
            double tableEndY = currentY;
            gfx.DrawRectangle(XPens.Black, 50, tableStartY, tableWidth, tableEndY - tableStartY);
            gfx.DrawLine(XPens.Black, 50 + col1Width, tableStartY, 50 + col1Width, tableEndY);
            gfx.DrawLine(XPens.Black, 50 + col1Width + col2Width, tableStartY, 50 + col1Width + col2Width, tableEndY);
            gfx.DrawLine(XPens.Black, 50 + col1Width + col2Width + col3Width, tableStartY, 50 + col1Width + col2Width + col3Width, tableEndY);
            gfx.DrawLine(XPens.Black, 50 + col1Width + col2Width + col3Width + col4Width, tableStartY, 50 + col1Width + col2Width + col3Width + col4Width, tableEndY);

            for (double y = tableStartY + rowHeight; y <= tableEndY; y += rowHeight)
            {
                gfx.DrawLine(XPens.Black, 50, y, 50 + tableWidth, y);
            }


            // Summary Section
            currentY += 10;
            decimal droitDeTimbre = _settingsService.GetDroitDuTimbrePriceAsync().Result; // 1% stamp duty (adjust as needed)

            decimal tva = quotation.TotalPrice - quotation.TotalPriceHT - droitDeTimbre; // Assuming TotalPrice is TTC and TotalPriceHT is HT
            decimal netAPayer = quotation.TotalPrice;

            gfx.DrawString($"TOTAL HT brut: {quotation.TotalPriceHT:F1}", smallFont, blackBrush, new XRect(50 + col1Width + col2Width + col3Width, currentY, col3Width + col4Width, rowHeight), XStringFormats.CenterRight);
            currentY += rowHeight;
            gfx.DrawString($"TOTAL TVA: {tva:F1}", smallFont, blackBrush, new XRect(50 + col1Width + col2Width + col3Width, currentY, col3Width + col4Width, rowHeight), XStringFormats.CenterRight);
            currentY += rowHeight;
            gfx.DrawString($"Droit de timbre: {droitDeTimbre:F1}", smallFont, blackBrush, new XRect(50 + col1Width + col2Width + col3Width, currentY, col3Width + col4Width, rowHeight), XStringFormats.CenterRight);
            currentY += rowHeight;
            gfx.DrawString($"Net à payer: {netAPayer:F1}", smallFont, blackBrush, new XRect(50 + col1Width + col2Width + col3Width, currentY, col3Width + col4Width, rowHeight), XStringFormats.CenterRight);

            // Footer: Draw the same design at the bottom
            string footerPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "Services", "impl", "footer.png");
            if (File.Exists(footerPath))
            {
                var footer = XImage.FromFile(footerPath);

                // Scale the image by 50% (to reduce it by 50%)
                double newWidth = footer.Width * 0.57;
                double newHeight = footer.Height * 0.57;

                // Calculate the X position to center the image horizontally with the resized dimensions
                double xPosition = (page.Width - newWidth - 25) / 2; // Centers the image based on the new width

                // Set the Y position to place the image at the bottom (with a margin of 10 units from the page bottom)
                double yPosition = page.Height - newHeight - 10; // Adjust the 10 as needed for margin from the bottom

                // Draw the footer image at the calculated position, with the resized dimensions
                gfx.DrawImage(footer, xPosition, yPosition, newWidth, newHeight);
            }

            // Save the PDF to a stream
            using var stream = new MemoryStream();
            document.Save(stream, false);
            return stream.ToArray();
        }
    }
}
