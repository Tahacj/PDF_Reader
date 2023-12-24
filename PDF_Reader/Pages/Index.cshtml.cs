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
            string fileName = "";
            if (fileUpload != null && fileUpload.Length > 0)
            {
                fileName = fileUpload.FileName;
                using (Stream stream = fileUpload.OpenReadStream())
                {
                    Button_Click(stream, fileName);
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
                    return File(new FileStream($"{fileName}-modified.pdf", FileMode.Open), "application/pdf", $"{fileName}-modified.pdf");
                }
            }
            return Page();
        }

        public void Button_Click(Stream file, string filename)
        {

            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(file);



            string invoiceNumer = "";
            string costumerId = "";
            string billTo = "";
            string shipTo = "";
            string shipDate = "";
            string order = "";
            var quantities = new List<string>();
            var products = new List<string>();
            var prices = new List<string>();
            var netPrices = new List<string>();
            string totalNetPrice = "";

            // Loop in the pages of the loaded PDF document
            for (int i = 0; i < (loadedDocument.Pages.Count); i++)
            {
                PdfPageBase page = loadedDocument.Pages[i];
                Console.WriteLine("width " + page.Size.Width + "\n hight " + page.Size.Height);
                // Extract text from the first page with bounds
                page.ExtractText(out TextLineCollection lineCollection);

                Console.WriteLine("width " + page.Size.Width + "\n hight " + page.Size.Height);
                PdfGraphics graphics = page.Graphics;

                //under armor
                //RectangleF invoiceBounds = new RectangleF(485, 24, 60, 10);
                //RectangleF costumerIdeBounds = new RectangleF(485, 35, 75, 10);
                //RectangleF billToBounds = new RectangleF(68, 190, 120, 35);
                //RectangleF shipToBounds = new RectangleF(360, 190, 150, 60);
                //RectangleF shipDateBounds = new RectangleF(306, 298, 143, 15);
                //RectangleF orderBounds = new RectangleF(450, 270, 144, 12);
                //RectangleF qtyBounds = new RectangleF(21, 330, 30, 200);
                //RectangleF productsBounds = new RectangleF(162, 330, 80, 200);
                //RectangleF priceBounds = new RectangleF(404, 330, 42, 200);
                //RectangleF netPriceBounds = new RectangleF(551, 331, 44, 238);
                //RectangleF totalNetPriceBounds = new RectangleF(533, 579, 60, 12);

                //under armor (Old 2019)
                //RectangleF invoiceBounds = new RectangleF(432, 58, 70, 10);
                //RectangleF costumerIdeBounds = new RectangleF(50, 305, 110, 10);
                //RectangleF billToBounds = new RectangleF(100, 137, 160, 65);
                //RectangleF shipToBounds = new RectangleF(405, 205, 160, 65);
                //RectangleF shipDateBounds = new RectangleF(330, 330, 83, 13);
                //RectangleF orderBounds = new RectangleF(432, 84, 70, 12);
                //RectangleF qtyBounds = new RectangleF(280, 372, 37, 450);
                //RectangleF productsBounds = new RectangleF(8, 372, 76, 450);
                //RectangleF priceBounds = new RectangleF(323, 372, 50, 450);
                //RectangleF netPriceBounds = new RectangleF(551, 331, 44, 238);
                //RectangleF totalNetPriceBounds = new RectangleF(533, 579, 60, 12);

                //skechers // test this is not working ritght
                //RectangleF invoiceBounds = new RectangleF(450, 130, 100, 13);
                //RectangleF costumerIdeBounds = new RectangleF(15, 240, 50, 15);
                //RectangleF billToBounds = new RectangleF(66, 152, 225, 62);
                //RectangleF shipToBounds = new RectangleF(330, 150, 225, 62);
                //RectangleF shipDateBounds = new RectangleF(339, 130, 106, 13);
                //RectangleF orderBounds = new RectangleF(208, 270, 105, 13);
                //RectangleF qtyBounds = new RectangleF(406, 350, 30, 400);
                //RectangleF productsBounds = new RectangleF(12, 346, 48, 400);
                //RectangleF priceBounds = new RectangleF(440, 350, 55, 400);
                //RectangleF netPriceBounds = new RectangleF(496, 330, 91, 429);
                //RectangleF totalNetPriceBounds = new RectangleF(503, 761, 87, 10);

                //Taylor made
                //RectangleF invoiceBounds = new RectangleF(260, 106, 60, 15);
                //RectangleF costumerIdeBounds = new RectangleF(120, 140, 60, 10);
                //RectangleF billToBounds = new RectangleF(36, 150, 190, 85);
                //RectangleF shipToBounds = new RectangleF(305, 150, 215, 85);
                //RectangleF shipDateBounds = new RectangleF(335, 275, 60, 23);
                //RectangleF orderBounds = new RectangleF(36, 275, 50, 20);
                //RectangleF qtyBounds = new RectangleF(188, 335, 25, 250);
                //RectangleF productsBounds = new RectangleF(35, 335, 40, 250);
                //RectangleF priceBounds = new RectangleF(300, 335, 46, 250);
                //RectangleF netPriceBounds = new RectangleF(485, 338, 50, 264);
                //RectangleF totalNetPriceBounds = new RectangleF(290, 637, 69, 20);

                //Callaway
                //RectangleF invoiceBounds = new RectangleF(21, 219, 60, 10); //
                //RectangleF costumerIdeBounds = new RectangleF(21, 244, 65, 10); //
                //RectangleF billToBounds = new RectangleF(51, 125, 182, 80);
                //RectangleF shipToBounds = new RectangleF(50, 30, 185, 83);
                //RectangleF shipDateBounds = new RectangleF(171, 220, 50, 10);
                //RectangleF orderBounds = new RectangleF(147, 244, 67, 10);
                //RectangleF qtyBounds = new RectangleF(330, 271, 25, 350);
                //RectangleF productsBounds = new RectangleF(16, 271, 25, 350);
                //RectangleF priceBounds = new RectangleF(390, 271, 40, 350);
                //RectangleF netPriceBounds = new RectangleF(516, 266, 55, 379);
                //RectangleF totalNetPriceBounds = new RectangleF(516, 648, 54, 12);

                //Pawakaddy
                //float stratPosX = lineCollection.TextLine[9].Bounds.Location.X;
                //float stratPosY = lineCollection.TextLine[9].Bounds.Location.Y;
                //RectangleF invoiceBounds = new RectangleF(107 + stratPosX, 16 + stratPosY, 57, 10);
                //RectangleF costumerIdeBounds = new RectangleF(107 + stratPosX, 53 + stratPosY, 53, 10);
                //RectangleF billToBounds = new RectangleF(20 + stratPosX, 120 + stratPosY, 155, 65);
                //RectangleF shipToBounds = new RectangleF(244 + stratPosX, 120 + stratPosY, 155, 65);
                //RectangleF shipDateBounds = new RectangleF(111 + stratPosX, 30 + stratPosY, 46, 10);
                //RectangleF orderBounds = new RectangleF(300 + stratPosX, 52 + stratPosY, 60, 12);
                //RectangleF qtyBounds = new RectangleF(385 + stratPosX, 220 + stratPosY, 27, 210);
                //RectangleF productsBounds = new RectangleF(-9 + stratPosX, 220 + stratPosY, 77, 210);
                //RectangleF priceBounds = new RectangleF(424 + stratPosX, 220 + stratPosY, 35, 210);
                //RectangleF netPriceBounds = new RectangleF(522 + stratPosX, 220 + stratPosY, 65, 324);
                //RectangleF totalNetPriceBounds = new RectangleF(513 + stratPosX, 563 + stratPosY, 50, 15);

                //Mizuno
                //RectangleF invoiceBounds = new RectangleF(522, 113, 66, 20);
                //RectangleF costumerIdeBounds = new RectangleF(77, 124, 42, 12);
                //RectangleF billToBounds = new RectangleF(18, 137, 177, 91);
                //RectangleF shipToBounds = new RectangleF(267, 137, 182, 94);
                //RectangleF shipDateBounds = new RectangleF(520, 196, 64, 16);
                //RectangleF orderBounds = new RectangleF(70, 304, 42, 10);
                //RectangleF qtyBounds = new RectangleF(430, 294, 28, 335);
                //RectangleF productsBounds = new RectangleF(12, 294, 55, 335);
                //RectangleF priceBounds = new RectangleF(465, 294, 40, 335);
                //RectangleF netPriceBounds = new RectangleF(537, 294, 51, 341);
                //RectangleF totalNetPriceBounds = new RectangleF(378, 685, 45, 13);

                //Cobra Puma
                //RectangleF invoiceBounds = new RectangleF(27, 302, 136, 13);
                //RectangleF costumerIdeBounds = new RectangleF(27, 270, 136, 13);
                //RectangleF billToBounds = new RectangleF(84, 105, 180, 93);
                //RectangleF shipToBounds = new RectangleF(386, 164, 149, 81);
                //RectangleF shipDateBounds = new RectangleF(178, 270, 118, 14);
                //RectangleF orderBounds = new RectangleF(311, 270, 133, 13);
                //RectangleF qtyBounds = new RectangleF(310, 338, 28, 320);
                //RectangleF productsBounds = new RectangleF(25, 338, 55, 320);
                //RectangleF priceBounds = new RectangleF(343, 338, 54, 320);
                //RectangleF netPriceBounds = new RectangleF(513, 336, 58, 328);
                //RectangleF totalNetPriceBounds = new RectangleF(309, 680, 90, 14);

                //Titleist
                //RectangleF invoiceBounds = new RectangleF(200, 225, 60, 10);
                //RectangleF costumerIdeBounds = new RectangleF(320, 225, 41, 10);
                //RectangleF billToBounds = new RectangleF(60, 115, 183, 76);
                //RectangleF shipToBounds = new RectangleF(330, 115, 175, 76);
                //RectangleF shipDateBounds = new RectangleF(495, 225, 53, 10);
                //RectangleF orderBounds = new RectangleF(440, 225, 56, 10);
                //RectangleF qtyBounds = new RectangleF(415, 280, 23, 380);
                //RectangleF productsBounds = new RectangleF(5, 280, 68, 380);
                //RectangleF priceBounds = new RectangleF(442, 281, 41, 380);
                //RectangleF netPriceBounds = new RectangleF(513, 280, 56, 384);
                //RectangleF totalNetPriceBounds = new RectangleF(513, 675, 56, 14);

                //MOTOCADDY
                //RectangleF invoiceBounds = new RectangleF(430, 89, 52, 12);
                //RectangleF costumerIdeBounds = new RectangleF(427, 110, 43, 13);
                //RectangleF billToBounds = new RectangleF(46, 179, 126, 70);
                //RectangleF shipToBounds = new RectangleF(50, 273, 125, 68);
                //RectangleF shipDateBounds = new RectangleF(428, 180, 53, 14);
                //RectangleF orderBounds = new RectangleF(495, 208, 56, 11);
                //RectangleF qtyBounds = new RectangleF(63, 354, 36, 34);
                //RectangleF productsBounds = new RectangleF(476, 354, 52, 34);
                //RectangleF priceBounds = new RectangleF(297, 353, 32, 36);
                //RectangleF netPriceBounds = new RectangleF(329, 354, 35, 34);
                //RectangleF totalNetPriceBounds = new RectangleF(520, 398, 41, 11);

                //MASTERS217
                //RectangleF invoiceBounds = new RectangleF(250, 18, 75, 20);
                //RectangleF costumerIdeBounds = new RectangleF(276, 65, 56, 16);
                //RectangleF billToBounds = new RectangleF(8, 39, 164, 77);
                //RectangleF shipToBounds = new RectangleF(5, 135, 170, 75);
                //RectangleF shipDateBounds = new RectangleF(275, 76, 72, 17);
                //RectangleF orderBounds = new RectangleF(276, 93, 80, 14);
                //RectangleF qtyBounds = new RectangleF(383, 231, 44, 480);
                //RectangleF productsBounds = new RectangleF(5, 232, 63, 471);
                //RectangleF priceBounds = new RectangleF(469, 233, 36, 476);
                //RectangleF netPriceBounds = new RectangleF(528, 233, 41, 524);
                //RectangleF totalNetPriceBounds = new RectangleF(515, 767, 53, 18);

                //ClevelandGolf
                RectangleF invoiceBounds = new RectangleF(486, 311, 52, 17);
                RectangleF costumerIdeBounds = new RectangleF(24, 354, 45, 10);
                RectangleF billToBounds = new RectangleF(35, 156, 195, 84);
                RectangleF shipToBounds = new RectangleF(368, 154, 195, 87);
                RectangleF shipDateBounds = new RectangleF(416, 311, 49, 16);
                RectangleF orderBounds = new RectangleF(32, 452, 50, 18);
                RectangleF qtyBounds = new RectangleF(248, 397, 42, 43);//
                RectangleF productsBounds = new RectangleF(28, 398, 64, 39);//
                RectangleF priceBounds = new RectangleF(297, 396, 33, 44);
                RectangleF netPriceBounds = new RectangleF(538, 397, 51, 44);
                RectangleF totalNetPriceBounds = new RectangleF(524, 686, 50, 11);

                if (i == 0)
                {
                    DrawRectangle(graphics, invoiceBounds, Color.Red);
                    DrawRectangle(graphics, costumerIdeBounds, Color.Blue);
                    DrawRectangle(graphics, billToBounds, Color.Green);
                    DrawRectangle(graphics, shipToBounds, Color.Gold);
                    DrawRectangle(graphics, shipDateBounds, Color.GreenYellow);
                    DrawRectangle(graphics, orderBounds, Color.HotPink);
                    DrawRectangle(graphics, netPriceBounds, Color.Black);
                    DrawRectangle(graphics, totalNetPriceBounds, Color.Black);
                }
                DrawRectangle(graphics, qtyBounds, Color.Orange);
                DrawRectangle(graphics, productsBounds, Color.Olive);
                DrawRectangle(graphics, priceBounds, Color.Purple);





                //Get the text provided in the bounds
                foreach (var txtLine in lineCollection.TextLine)
                {
                    foreach (TextWord word in txtLine.WordCollection)
                    {
                        if (IsIntersected(invoiceBounds, word.Bounds) && i == 0)
                            invoiceNumer = word.Text;
                        if (IsIntersected(costumerIdeBounds, word.Bounds) && i == 0)
                            costumerId = word.Text;
                        if (IsIntersected(billToBounds, word.Bounds) && i == 0)
                            billTo += word.Text;
                        if (IsIntersected(shipToBounds, word.Bounds) && i == 0)
                            shipTo += word.Text;
                        if (IsIntersected(shipDateBounds, word.Bounds) && i == 0)
                            shipDate += word.Text;
                        if (IsIntersected(orderBounds, word.Bounds) && i == 0)
                            order += word.Text;
                        if (IsIntersected(qtyBounds, word.Bounds))
                            quantities.Add(word.Text);
                        if (IsIntersected(productsBounds, word.Bounds))
                            products.Add(word.Text);
                        if (IsIntersected(priceBounds, word.Bounds))
                            prices.Add(word.Text);
                        if (IsIntersected(netPriceBounds, word.Bounds))
                            netPrices.Add(word.Text);
                        if (IsIntersected(totalNetPriceBounds, word.Bounds))
                            totalNetPrice += word.Text;
                    }
                }

            }
            using (FileStream outputFileStream = new FileStream($"{filename}-modified.pdf", FileMode.Create))
            {
                loadedDocument.Save(outputFileStream);
            }
            loadedDocument.Close(true);

            string data = "";

            data = "Invoice Number: " + invoiceNumer;
            data += "\n\nCostumer ID: " + costumerId;
            data += "\n\nBill To: " + billTo;
            data += "\n\nShip To: " + shipTo;
            data += "\n\nShip Date: " + shipDate;
            data += "\n\nOrder: " + order;
            data += "\n\nTotal Net Price: " + totalNetPrice;
            data += "\n\n------------\nQTY:";
            foreach (var qty in quantities)
                data += "\n\n" + qty;
            data += "\n\n------------\nProducts:";
            foreach (var p in products)
                data += "\n\n" + p;
            data += "\n\n------------\nPrices:";
            foreach (var p in prices)
                data += "\n\n" + p;
            data += "\n\n------------\nNet Prices:";
            foreach (var net in netPrices)
                data += "\n\n" + net;
            Console.WriteLine(data);
            Validate(quantities, netPrices, totalNetPrice);

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
        private void Validate(List<string> QTY, List<string> Price, string totalNet)
        {
            float total = 0;
            if (Price.LongCount() > 0)
            {

                for (int i = 0; i < Price.LongCount(); i++)
                {
                    if (!string.IsNullOrWhiteSpace(Price[i]))
                    {
                        total += (float.Parse(Price[i].Trim()));

                    }
                }
                if (float.Parse(totalNet.Trim()) == total)
                {
                    Console.WriteLine("\n\n VALID \n\n");
                }
                else
                {
                    Console.WriteLine("\n\nNOT ACCEPTABLE\n\n");
                }
            }
            Console.WriteLine("TOTAL IS EQUAL TO :" + total);

        }
    }
}
