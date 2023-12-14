using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Hosting.Server;
using System;
using System.IO;


namespace PDF_Reader.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostPdfText(IFormFile fileUpload)
        {
            if (fileUpload != null && fileUpload.Length > 0)
            {
                // Get the file name
                //var fileName = System.IO.Path.GetFileName(fileUpload.FileName);

                //// Get the path
                //var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), fileName);

                using (Stream stream = fileUpload.OpenReadStream())
                {

                    // Now you have the path, you can process it as needed
                    //string path = "file:///M:/taha_programing_file/C%23/PDF%20Reader/Test%20Files/11428111130.pdf";
                    PdfReader reader = new PdfReader(stream);
                    string text = string.Empty;

                    for (int page = 1; page <= reader.NumberOfPages; page++)
                    {
                        text += PdfTextExtractor.GetTextFromPage(reader, page);
                    }

                    Console.WriteLine(text);

                    reader.Close();
                }
            }
            return Page();
        }
    }
}
