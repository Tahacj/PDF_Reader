using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.parser;
//using System.Reflection.PortableExecutable;
//using Microsoft.AspNetCore.Hosting.Server;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Drawing;
using System;
using System.IO;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;


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
				

				using (Stream stream = fileUpload.OpenReadStream())
				{
					Button_Click(stream);
					// Now you have the path, you can process it as needed
					//string path = "file:///M:/taha_programing_file/C%23/PDF%20Reader/Test%20Files/11428111130.pdf";
					//PdfReader reader = new PdfReader(stream);
					//string text = string.Empty;

					//for (int page = 1; page <= reader.NumberOfPages; page++)
					//{
					//	text += PdfTextExtractor.GetTextFromPage(reader, page);
					//}

					//Console.WriteLine(text);

					//reader.Close();
				}
			}
            return File(new FileStream("modified.pdf", FileMode.Open), "application/pdf", "modified.pdf");
        }

		public void Button_Click(Stream file)
		{

            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(file);
			// Get the first page of the loaded PDF document
			PdfPageBase page = loadedDocument.Pages[0];
			// Extract text from the first page with bounds
			page.ExtractText(out TextLineCollection lineCollection);

			Console.WriteLine("width " + page.Size.Width + "\n hight " + page.Size.Height);
            PdfGraphics graphics = page.Graphics;


            //under armor
            //RectangleF invoiceBounds = new RectangleF(485, 24, 60, 10);
            //RectangleF costumerIdeBounds = new RectangleF(485, 35, 75, 10);
            //RectangleF billToBounds = new RectangleF(68, 190, 120, 35);
            //RectangleF shipToBounds = new RectangleF(360, 190, 150, 60);
            //RectangleF shipDateBounds = new RectangleF(378, 304, 70, 15);
            //RectangleF orderBounds = new RectangleF(512, 275, 82, 12);
            //RectangleF qtyBounds = new RectangleF(21, 330, 30, 200);
            //RectangleF productsBounds = new RectangleF(162, 330, 80, 200);
            //RectangleF priceBounds = new RectangleF(404, 330, 42, 200);

            //skechers // test this is not working ritght
            //RectangleF invoiceBounds = new RectangleF(450, 130, 100, 13);
            //RectangleF costumerIdeBounds = new RectangleF(15, 240, 50, 15);
            //RectangleF billToBounds = new RectangleF(117, 152, 225, 62);
            //RectangleF shipToBounds = new RectangleF(330, 150, 225, 62);
            //RectangleF shipDateBounds = new RectangleF(378, 304, 70, 15); //
            //RectangleF orderBounds = new RectangleF(315, 270, 105, 13);
            //RectangleF qtyBounds = new RectangleF(406, 329, 30, 425); 
            //RectangleF productsBounds = new RectangleF(162, 330, 80, 200); //
            //RectangleF priceBounds = new RectangleF(435, 332, 55, 425);

            //Taylor made
            //RectangleF invoiceBounds = new RectangleF(260, 106, 60, 15);
            //RectangleF costumerIdeBounds = new RectangleF(120, 140, 60, 10);
            //RectangleF billToBounds = new RectangleF(36, 150, 190, 85);
            //RectangleF shipToBounds = new RectangleF(305, 150, 215, 85);
            //RectangleF shipDateBounds = new RectangleF(335, 275, 60, 23);
            //RectangleF orderBounds = new RectangleF(36, 275, 50, 20);
            //RectangleF qtyBounds = new RectangleF(188, 335, 25, 250);
            //RectangleF productsBounds = new RectangleF(75, 335, 102, 250);
            //RectangleF priceBounds = new RectangleF(300, 335, 46, 250);

            //Callaway
            //RectangleF invoiceBounds = new RectangleF(21, 219, 60, 10); //
            //RectangleF costumerIdeBounds = new RectangleF(21, 244, 65, 10); //
            //RectangleF billToBounds = new RectangleF(53, 128, 200, 80);
            //RectangleF shipToBounds = new RectangleF(50, 30, 185, 83);
            //RectangleF shipDateBounds = new RectangleF(171, 220, 50, 10);
            //RectangleF orderBounds = new RectangleF(147, 244, 67, 10);
            //RectangleF qtyBounds = new RectangleF(350, 300, 25, 330);
            //RectangleF productsBounds = new RectangleF(135, 300, 173, 330);
            //RectangleF priceBounds = new RectangleF(390, 300, 35, 330);

            //Pawakaddy
            //RectangleF invoiceBounds = new RectangleF(128, 42, 57, 10); 
            //RectangleF costumerIdeBounds = new RectangleF(132, 79, 53, 10); 
            //RectangleF billToBounds = new RectangleF(40, 146, 155, 65);
            //RectangleF shipToBounds = new RectangleF(264, 147, 155, 65);
            //RectangleF shipDateBounds = new RectangleF(131, 56, 46, 10);
            //RectangleF orderBounds = new RectangleF(319, 78, 60, 12);
            //RectangleF qtyBounds = new RectangleF(405, 246, 27, 210);
            //RectangleF productsBounds = new RectangleF(135, 300, 173, 330);//
            //RectangleF priceBounds = new RectangleF(444, 246, 35, 210);

            //Pawakaddy
            //RectangleF invoiceBounds = new RectangleF(128, 42, 57, 10);
            //RectangleF costumerIdeBounds = new RectangleF(132, 79, 53, 10);
            //RectangleF billToBounds = new RectangleF(40, 146, 155, 65);
            //RectangleF shipToBounds = new RectangleF(264, 147, 155, 65);
            //RectangleF shipDateBounds = new RectangleF(131, 56, 46, 10);
            //RectangleF orderBounds = new RectangleF(319, 78, 60, 12);
            //RectangleF qtyBounds = new RectangleF(405, 246, 27, 210);
            //RectangleF productsBounds = new RectangleF(135, 300, 173, 330);//
            //RectangleF priceBounds = new RectangleF(444, 246, 35, 210);

            //MIZUNO
            RectangleF invoiceBounds = new RectangleF(128, 42, 57, 10);
            RectangleF costumerIdeBounds = new RectangleF(132, 79, 53, 10);
            RectangleF billToBounds = new RectangleF(40, 146, 155, 65);
            RectangleF shipToBounds = new RectangleF(264, 147, 155, 65);
            RectangleF shipDateBounds = new RectangleF(131, 56, 46, 10);
            RectangleF orderBounds = new RectangleF(319, 78, 60, 12);
            RectangleF qtyBounds = new RectangleF(405, 246, 27, 210);
            RectangleF productsBounds = new RectangleF(135, 300, 173, 330);//
            RectangleF priceBounds = new RectangleF(444, 246, 35, 210);

            DrawRectangle(graphics, invoiceBounds, Color.Red);
            DrawRectangle(graphics, costumerIdeBounds, Color.Blue);
            DrawRectangle(graphics, billToBounds, Color.Green);
            DrawRectangle(graphics, shipToBounds, Color.Gold);
            DrawRectangle(graphics, shipDateBounds, Color.GreenYellow);
            DrawRectangle(graphics, orderBounds, Color.HotPink);
            DrawRectangle(graphics, qtyBounds, Color.Orange);
            DrawRectangle(graphics, productsBounds, Color.Olive);
            DrawRectangle(graphics, priceBounds, Color.Purple);

            using (FileStream outputFileStream = new FileStream("modified.pdf", FileMode.Create))
            {
                loadedDocument.Save(outputFileStream);
            }



            string invoiceNumer = "";
			string costumerId = "";
			string billTo = "";
			string shipTo = "";
            string shipDate = "";
			string order = "";
            var quantities = new List<string>();
			var products = new List<string>();
			var prices = new List<string>();
			//Get the text provided in the bounds
			foreach (var txtLine in lineCollection.TextLine)
			{
				foreach (TextWord word in txtLine.WordCollection)
				{
					if (IsIntersected(invoiceBounds, word.Bounds))
						invoiceNumer = word.Text;
                    if (IsIntersected(costumerIdeBounds, word.Bounds))
                        costumerId = word.Text;
                    if (IsIntersected(billToBounds, word.Bounds))
                        billTo += word.Text;
                    if (IsIntersected(shipToBounds, word.Bounds))
                        shipTo += word.Text;
                    if (IsIntersected(shipDateBounds, word.Bounds))
						shipDate += word.Text;
					if (IsIntersected(orderBounds, word.Bounds))
						order += word.Text;
					if (IsIntersected(qtyBounds, word.Bounds))
						quantities.Add(word.Text);
					if (IsIntersected(productsBounds, word.Bounds))
						products.Add(word.Text);
					if (IsIntersected(priceBounds, word.Bounds))
						prices.Add(word.Text);
				}
			}
			loadedDocument.Close(true);

			string data = "";

			data = "-Invoice Number: " + invoiceNumer;
            data += "\n\n-Costumer ID: " + costumerId;
            data += "\n\n-Bill To: " + billTo;
            data += "\n\n-Ship To: " + shipTo;
            data += "\n\n-Ship Date: " + shipDate;
			data += "\n\n-Order: " + order;
			data += "\n\n------------\nQTY:";
			foreach (var qty in quantities)
				data += "\n\n-" + qty;
			data += "\n\n------------\nProducts:";
			foreach (var p in products)
				data += "\n\n-" + p;
			data += "\n\n------------\nPrices:";
			foreach (var p in prices)
				data += "\n\n-" + p;

            Console.WriteLine(data);
		}

		internal bool IsIntersected(RectangleF rect1, RectangleF rect2)
		{
			if (rect2.X < rect1.X + rect1.Width && rect1.X < rect2.X + rect2.Width && rect2.Y < rect1.Y + rect1.Height)
			{
				return rect1.Y < rect2.Y + rect2.Height;
			}

			return false;
		}

        private void DrawRectangle(PdfGraphics graphics, RectangleF bounds, Color color)
        {
            PdfPen pen = new PdfPen(new PdfColor(color), 2f);
            graphics.DrawRectangle(pen, bounds);
        }
    }
}
