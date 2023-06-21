using iText.Forms;
using iText.Kernel.Pdf;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/pdf")]
    public class PdfController : ControllerBase
    {
        [HttpGet("fillform")]
        public IActionResult FillForm(string name)
        {
            // Specify the path to the PDF template
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "template.pdf");

            // Populate the PDF form with the given name
            byte[] filledPdf = PopulatePdfForm(templatePath, name);

            // Return the filled PDF file
            return File(filledPdf, "application/pdf", "filled_form.pdf");
        }

        private byte[] PopulatePdfForm(string templatePath, string name)
        {
            var output = new MemoryStream();

            // Load the PDF template
            using (var reader = new PdfReader(templatePath))
            {
                // Create a PdfDocument from the template
                using (var pdfDoc = new PdfDocument(reader, new PdfWriter(output)))
                {
                    // Get the form fields from the template
                    var form = PdfAcroForm.GetAcroForm(pdfDoc, true);

                    // Set the value for the "studentName" field
                    var fieldPath = "studentName";
                    var field = form.GetField(fieldPath);
                    field.SetValue(name);

                    // Close the document
                    pdfDoc.Close();
                }
            }

            // Return the modified PDF file as a byte array
            return output.ToArray();
        }
    }
}
