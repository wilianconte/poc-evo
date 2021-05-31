using EvoPdf.HtmlToPdfClient;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Teste.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
             
        [HttpGet]
        public IActionResult Get()
        {
            //HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter(true, serverHttpUrl);

            // Create the PDF document where to add the HTML documents
            Document pdfDocument = new Document(true, "http://a2bfa28f5b8e6ccdb.awsglobalaccelerator.com");

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            pdfDocument.LicenseKey = "4W9+bn19bn5ue2B+bn1/YH98YHd3d3c=";

            // Create a PDF page where to add the first HTML
            var firstPdfPage = pdfDocument.AddPage();

            try
            {
                //Add Header
                AddHeader("Documento 01 - Header 01", pdfDocument, true);

                HtmlToPdfElement firstHtml = new HtmlToPdfElement("<h2>Teste 1</h2>", null);

                // Optionally set a delay before conversion to allow asynchonous scripts to finish
                firstHtml.ConversionDelay = 2;

                // Add the first HTML to PDF document
                firstPdfPage.AddElement(firstHtml);

                //-----------------------------------------------------------------------

                pdfDocument.AppendDocument(GetDocument("Documento 02"));

                //-----------------------------------------------------------------------


                // Save the PDF document in a memory buffer
                byte[] pdfBytes = pdfDocument.Save();

                return new FileStreamResult(new MemoryStream(pdfBytes), "application/pdf");


            }
            finally
            {
                // Close the PDF document
               // pdfDocument.Close();
            }
        }

        private byte[] GetDocument(string texto)
        {
            // Create the PDF document where to add the HTML documents
            Document pdfDocument = new Document(true, "http://a2bfa28f5b8e6ccdb.awsglobalaccelerator.com");

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            pdfDocument.LicenseKey = "4W9+bn19bn5ue2B+bn1/YH98YHd3d3c=";

            // Create a PDF page where to add the first HTML
            var firstPdfPage = pdfDocument.AddPage();

            try
            {
                //Add Header
                AddHeader($" {texto} - Header 01", pdfDocument, true);

                HtmlToPdfElement firstHtml = new HtmlToPdfElement("<h2>Teste 1</h2>", null);

                // Optionally set a delay before conversion to allow asynchonous scripts to finish
                firstHtml.ConversionDelay = 2;

                // Add the first HTML to PDF document
                firstPdfPage.AddElement(firstHtml);

                return pdfDocument.Save();
            }
            finally
            {
                // Close the PDF document
               // pdfDocument.Close();
            }
        }

        /// <summary>
        /// Add a header to document
        /// </summary>
        /// <param name="pdfDocument">The PDF document object</param>
        /// <param name="drawHeaderLine">A flag indicating if a line should be drawn at the bottom of the header</param>
        private void AddHeader (string texto, Document pdfDocument, bool drawHeaderLine)
        {            

            // Create the document footer template
            pdfDocument.AddHeaderTemplate(60);

            // Create a HTML element to be added in header
            HtmlToPdfElement headerHtml = new HtmlToPdfElement($"<h1>{texto}</h1>", null);

            // Set the HTML element to fit the container height
            headerHtml.FitHeight = true;

            // Add HTML element to header
            pdfDocument.Header.AddElement(headerHtml);

            if (drawHeaderLine)
            {
                float headerWidth = pdfDocument.Header.Width;
                float headerHeight = pdfDocument.Header.Height;

                // Create a line element for the bottom of the header
                LineElement headerLine = new LineElement(0, headerHeight - 1, headerWidth, headerHeight - 1);

                // Set line color
                headerLine.ForeColor = RgbColor.Gray;

                // Add line element to the bottom of the header
                pdfDocument.Header.AddElement(headerLine);
            }
        }

        /// <summary>
        /// Add a footer to document
        /// </summary>
        /// <param name="pdfDocument">The PDF document object</param>
        /// <param name="addPageNumbers">A flag indicating if the page numbering is present in footer</param>
        /// <param name="drawFooterLine">A flag indicating if a line should be drawn at the top of the footer</param>
        private void AddFooter(Document pdfDocument, bool addPageNumbers, bool drawFooterLine)
        {            

            // Create the document footer template
            pdfDocument.AddFooterTemplate(60);

            // Set footer background color
            RectangleElement backColorRectangle = new RectangleElement(0, 0, pdfDocument.Footer.Width, pdfDocument.Footer.Height);
            backColorRectangle.BackColor = RgbColor.WhiteSmoke;
            pdfDocument.Footer.AddElement(backColorRectangle);

            // Create a HTML element to be added in footer
            HtmlToPdfElement footerHtml = new HtmlToPdfElement("<h1>Hellow Footer</h1>");

            // Set the HTML element to fit the container height
            footerHtml.FitHeight = true;

            // Add HTML element to footer
            pdfDocument.Footer.AddElement(footerHtml);

            // Add page numbering
            if (addPageNumbers)
            {
                // Create a text element with page numbering place holders &p; and & P;
                TextElement footerText = new TextElement(0, 30, "Page &p; of &P;  ", new PdfFont("Times New Roman", 10, true));


                // Align the text at the right of the footer
                footerText.TextAlign = HorizontalTextAlign.Right;

                // Set page numbering text color
                footerText.ForeColor = RgbColor.Navy;

                // Embed the text element font in PDF
                footerText.EmbedSysFont = true;

                // Add the text element to footer
                pdfDocument.Footer.AddElement(footerText);
            }

            if (drawFooterLine)
            {
                float footerWidth = pdfDocument.Footer.Width;

                // Create a line element for the top of the footer
                LineElement footerLine = new LineElement(0, 0, footerWidth, 0);

                // Set line color
                footerLine.ForeColor = RgbColor.Gray;

                // Add line element to the bottom of the footer
                pdfDocument.Footer.AddElement(footerLine);
            }
        }
    }
}
