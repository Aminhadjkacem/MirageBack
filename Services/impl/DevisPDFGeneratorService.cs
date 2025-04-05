using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;
using DHP_Manager.Models.input;
using DHP_Manager.Services.Interface;
using DHP_Manager.Models.ENUM;

namespace DHP_Manager.Services.impl
{
    public class DevisPDFGeneratorService : IDevisPDFGeneratorService
    {
        private readonly ISettingsService _settingsService;
        private readonly IProductService _productService;
        private readonly INormalCustomerService _normalCustomerService;
        private readonly ICompanyCustomerService _companyCustomerService;

        public DevisPDFGeneratorService(
            ISettingsService settingsService,
            IProductService productService,
            INormalCustomerService normalCustomerService,
            ICompanyCustomerService companyCustomerService)
        {
            _settingsService = settingsService;
            _productService = productService;
            _normalCustomerService = normalCustomerService;
            _companyCustomerService = companyCustomerService;
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

            // Header: Draw logo
            string logoPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "Services", "impl", "logo.png");
            if (File.Exists(logoPath))
            {
                var logo = XImage.FromFile(logoPath);
                if (logo != null && logo.Height > 0)
                {
                    int newHeight = 40;
                    int newWidth = (int)((double)(logo.Width) * newHeight / logo.Height);
                    gfx.DrawImage(logo, 50, 20, newWidth, newHeight);
                }
            }

            int currentYear = quotation.QuotationCreatedAt.Value.Year;

            // Invoice number and quotation ID
            string invoiceNumberText = $"Devis";
            string quotationIdText = $"N°: 00{quotation.Id}/{currentYear}";

            XFont fontInvoiceNumber = new XFont("Arial", 20);
            XFont fontQuotationId = new XFont("Arial", 10);
            var textWidthInvoice = gfx.MeasureString(invoiceNumberText, fontInvoiceNumber).Width;
            var textWidthQuotationId = gfx.MeasureString(quotationIdText, fontQuotationId).Width;

            gfx.DrawString(invoiceNumberText, fontInvoiceNumber, orangeBrush, new XRect(page.Width - textWidthInvoice - 50, 30, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(quotationIdText, fontQuotationId, blackBrush, new XRect(page.Width - textWidthQuotationId - 50, 30 + fontInvoiceNumber.Height + 5, page.Width, page.Height), XStringFormats.TopLeft);

            // Header line
            var penBlue = new XPen(XColors.DarkBlue, 1);
            var penOrange = new XPen(XColors.Orange, 1);
            gfx.DrawLine(penBlue, 50, 70, page.Width / 2, 70);
            gfx.DrawLine(penOrange, page.Width / 2, 70, page.Width - 50, 70);

            // Company Details (left side)
            double companyDetailsY = 80;
            double companyDetailsX = 50;
            double lineHeight = 12;

            // Fetch company details from settings service
            string companyTel = _settingsService.GetCompanyTelAsync().Result;
            string companyFiscal = _settingsService.GetCompanyFiscalAsync().Result;
            string bankName = _settingsService.GetBankNameAsync().Result;
            string companyRIB = _settingsService.GetCompanyRibAsync().Result;
            string invoiceDate = DateTime.Now.ToString("dd/MM/yyyy");

            // Measure company details width
            double telWidth = gfx.MeasureString($"TEL: {companyTel}", smallFont).Width;
            double fiscalWidth = gfx.MeasureString($"MF: {companyFiscal}", smallFont).Width;
            double bankWidth = gfx.MeasureString($"Banque: {bankName}", smallFont).Width;
            double ribWidth = gfx.MeasureString($"RIB: {companyRIB}", smallFont).Width;
            double dateWidth = gfx.MeasureString($"Date: {invoiceDate}", smallFont).Width;

            double maxCompanyWidth = Math.Max(telWidth, Math.Max(fiscalWidth, Math.Max(bankWidth, Math.Max(ribWidth, dateWidth))));
            double companyLineEndX = companyDetailsX + maxCompanyWidth + 10;

            // Draw company details
            gfx.DrawString($"TEL: {companyTel}", smallFont, blackBrush, new XPoint(companyDetailsX, companyDetailsY), XStringFormats.TopLeft);
            companyDetailsY += lineHeight;
            gfx.DrawString($"MF: {companyFiscal}", smallFont, blackBrush, new XPoint(companyDetailsX, companyDetailsY), XStringFormats.TopLeft);
            companyDetailsY += lineHeight;
            gfx.DrawString($"Banque: {bankName}", smallFont, blackBrush, new XPoint(companyDetailsX, companyDetailsY), XStringFormats.TopLeft);
            companyDetailsY += lineHeight;
            gfx.DrawString($"RIB: {companyRIB}", smallFont, blackBrush, new XPoint(companyDetailsX, companyDetailsY), XStringFormats.TopLeft);
            companyDetailsY += lineHeight;

            double companyLineY = companyDetailsY + 5;
            double companyMidPoint = companyDetailsX + (companyLineEndX - companyDetailsX) / 2;
            gfx.DrawLine(penBlue, companyDetailsX, companyLineY, companyMidPoint, companyLineY);
            gfx.DrawLine(penOrange, companyMidPoint, companyLineY, companyLineEndX, companyLineY);

            companyDetailsY += lineHeight + 5;
            gfx.DrawString($"Date: {invoiceDate}", smallFont, blackBrush, new XPoint(companyDetailsX, companyDetailsY), XStringFormats.TopLeft);

            // Customer Details (right side, in a box, left-aligned)
            double customerDetailsY = 80; // Same Y as company details for symmetry
            double customerDetailsX = page.Width - 50 - maxCompanyWidth - 20; // Position to match company details width + padding
            double customerLineHeight = 12;
            double padding = 5; // Padding inside the box

            // Fetch customer details based on CustomerType
            string clientName = "";
            string address = "";
            string ville = "";
            string matriculeFiscal = "";

            if (quotation.CustomerType == CustomerType.COMPANY)
            {
                var companyCustomer = _companyCustomerService.GetByIdAsync(quotation.CustomerId).Result;
                if (companyCustomer != null)
                {
                    clientName = companyCustomer.CompanyName;
                    address = companyCustomer.Address;
                    ville = companyCustomer.Ville;
                    matriculeFiscal = companyCustomer.MatriculeFiscal;
                }
            }
            else if (quotation.CustomerType == CustomerType.NORMAL)
            {
                var normalCustomer = _normalCustomerService.GetByIdAsync(quotation.CustomerId).Result;
                if (normalCustomer != null)
                {
                    clientName = normalCustomer.FullName;
                    address = normalCustomer.Address;
                }
            }

            // Measure customer details width
            double clientWidth = gfx.MeasureString($"Client: {clientName}", smallFont).Width;
            double addressWidth = gfx.MeasureString($"Adresse: {address}", smallFont).Width;
            double villeWidth = quotation.CustomerType == CustomerType.COMPANY ? gfx.MeasureString($"Ville: {ville}", smallFont).Width : 0;
            double mfWidth = quotation.CustomerType == CustomerType.COMPANY ? gfx.MeasureString($"MF: {matriculeFiscal}", smallFont).Width : 0;

            double maxCustomerWidth = Math.Max(clientWidth, Math.Max(addressWidth, Math.Max(villeWidth, mfWidth)));
            double boxWidth = maxCustomerWidth + 2 * padding; // Add padding on both sides
            double boxHeight = quotation.CustomerType == CustomerType.COMPANY ? (4 * customerLineHeight + 2 * padding) : (2 * customerLineHeight + 2 * padding); // Height based on number of lines

            // Draw the box
            double boxX = customerDetailsX - padding; // Start of the box
            double boxY = customerDetailsY - padding; // Top of the box
            gfx.DrawRectangle(XPens.Black, boxX, boxY, boxWidth, boxHeight);

            // Draw customer details (left-aligned inside the box)
            double textX = customerDetailsX; // Left-aligned inside the box
            double textY = customerDetailsY; // Start text inside the box

            gfx.DrawString($"Client: {clientName}", smallFont, blackBrush, new XPoint(textX, textY), XStringFormats.TopLeft);
            textY += customerLineHeight;
            gfx.DrawString($"Adresse: {address}", smallFont, blackBrush, new XPoint(textX, textY), XStringFormats.TopLeft);

            if (quotation.CustomerType == CustomerType.COMPANY)
            {
                textY += customerLineHeight;
                gfx.DrawString($"Ville: {ville}", smallFont, blackBrush, new XPoint(textX, textY), XStringFormats.TopLeft);
                textY += customerLineHeight;
                gfx.DrawString($"MF: {matriculeFiscal}", smallFont, blackBrush, new XPoint(textX, textY), XStringFormats.TopLeft);
            }

            // Update customerDetailsY to the bottom of the box for table positioning
            customerDetailsY = boxY + boxHeight + padding;

            // Table setup (adjust start Y to account for the taller section)
            double tableStartY = Math.Max(companyDetailsY, customerDetailsY) + 20; // Use the taller section's Y
            double tableWidth = page.Width - 100;
            double col1Width = tableWidth * 0.3; // Désignation
            double col2Width = tableWidth * 0.1; // Qté
            double col3Width = tableWidth * 0.2; // P.U.H.T
            double col4Width = tableWidth * 0.2; // Montant H.T
            double col5Width = tableWidth * 0.1; // TVA %
            double rowHeight = 20;
            double currentY = tableStartY;

            // Table Header
            gfx.DrawRectangle(new XSolidBrush(XColors.Orange), 50, currentY, tableWidth, rowHeight);
            gfx.DrawString("Désignation", font, blackBrush, new XRect(50, currentY, col1Width, rowHeight), XStringFormats.Center);
            gfx.DrawString("Qté", font, blackBrush, new XRect(50 + col1Width, currentY, col2Width, rowHeight), XStringFormats.Center);
            gfx.DrawString("P.U.H.T", font, blackBrush, new XRect(50 + col1Width + col2Width, currentY, col3Width, rowHeight), XStringFormats.Center);
            gfx.DrawString("Montant H.T", font, blackBrush, new XRect(50 + col1Width + col2Width + col3Width, currentY, col4Width, rowHeight), XStringFormats.Center);
            currentY += rowHeight;

            decimal profitMargin = _settingsService.GetProfitMarginAsync().Result;

            // Table Data
            decimal totalHTBrut = 0m;
            if (quotation.Designations != null && quotation.Designations.Any())
            {
                foreach (var designation in quotation.Designations)
                {
                    if (designation == null || string.IsNullOrEmpty(designation.Name))
                    {
                        continue;
                    }

                    // Calculate base price for this designation
                    decimal designationBasePrice = 0m;
                    if (designation.Products != null)
                    {
                        foreach (var productInput in designation.Products)
                        {
                            var product = _productService.GetProductByIdAsync(productInput.ProductId).Result;
                            if (product != null)
                            {
                                designationBasePrice += product.Price * productInput.Quantity;
                            }
                        }
                    }

                    // Apply designation quantity and profit margin
                    decimal designationHT = designationBasePrice * designation.Quantity * (1 + profitMargin);

                    totalHTBrut += designationHT;

                    double offsetX = 18;
                    string unitPriceText = (designationBasePrice * (1 + profitMargin)).ToString("F1");
                    string totalHTText = designationHT.ToString("F1");

                    // Draw row
                    gfx.DrawString(designation.Name, smallFont, blackBrush, new XPoint(50 + offsetX, currentY + rowHeight / 2), XStringFormats.CenterLeft);
                    gfx.DrawString(designation.Quantity.ToString(), smallFont, blackBrush, new XPoint(50 + col1Width + offsetX, currentY + rowHeight / 2), XStringFormats.Center);
                    gfx.DrawString(unitPriceText, smallFont, blackBrush, new XPoint(50 + col1Width + col2Width + offsetX, currentY + rowHeight / 2), XStringFormats.Center);
                    gfx.DrawString(totalHTText, smallFont, blackBrush, new XPoint(50 + col1Width + col2Width + col3Width + offsetX, currentY + rowHeight / 2), XStringFormats.Center);
                    currentY += rowHeight;
                }
            }

            // Table borders
            double tableEndY = currentY;
            gfx.DrawRectangle(XPens.Black, 50, tableStartY, tableWidth, tableEndY - tableStartY);
            gfx.DrawLine(XPens.Black, 50 + col1Width, tableStartY, 50 + col1Width, tableEndY);
            gfx.DrawLine(XPens.Black, 50 + col1Width + col2Width, tableStartY, 50 + col1Width + col2Width, tableEndY);
            gfx.DrawLine(XPens.Black, 50 + col1Width + col2Width + col3Width, tableStartY, 50 + col1Width + col2Width + col3Width, tableEndY);

            for (double y = tableStartY + rowHeight; y <= tableEndY; y += rowHeight)
            {
                gfx.DrawLine(XPens.Black, 50, y, 50 + tableWidth, y);
            }

            // Summary Section
            currentY += 10;
            decimal tvaMargin = _settingsService.GetTVAMarginAsync().Result;
            decimal droitDeTimbre = _settingsService.GetDroitDuTimbrePriceAsync().Result;

            decimal priceWithProfit = totalHTBrut;
            decimal tvaAmount = priceWithProfit * tvaMargin;
            decimal netAPayer = priceWithProfit + tvaAmount + droitDeTimbre;

            gfx.DrawString($"TOTAL HT brut: {totalHTBrut:F1}", smallFont, blackBrush, new XRect(50 + col1Width + col2Width + col3Width, currentY, col3Width + col4Width, rowHeight), XStringFormats.CenterRight);
            currentY += rowHeight;
            // Footer
            string footerPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "Services", "impl", "footer.png");
            if (File.Exists(footerPath))
            {
                var footer = XImage.FromFile(footerPath);
                double newWidth = footer.Width * 0.57;
                double newHeight = footer.Height * 0.57;
                double xPosition = (page.Width - newWidth - 25) / 2;
                double yPosition = page.Height - newHeight - 10;
                gfx.DrawImage(footer, xPosition, yPosition, newWidth, newHeight);
            }

            // Save to stream
            using var stream = new MemoryStream();
            document.Save(stream, false);
            return stream.ToArray();
        }
    }
}