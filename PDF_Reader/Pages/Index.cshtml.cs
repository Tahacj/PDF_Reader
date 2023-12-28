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
using Org.BouncyCastle.Crypto.Tls;


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


            string brandName = "";
            string invoiceNumer = "";
            string costumerId = "";
            string billTo = "";
            string shipTo = "";
            string shipDate = "";
            string order = "";
            var quantities = new List<string>();
            var products = new List<string>();
            var prices = new List<string>();
            var discountAmount = new List<string>();
            string totalNetPrice = "";
            // for Cllaway
            var qtyRectangles = new List<RectangleF>();
            var priceRectangles = new List<RectangleF>();
            var discountRectangles = new List<RectangleF>();
            var productsRectangles = new List<RectangleF>();
            TextLineCollection pageToSend;

            // Loop in the pages of the loaded PDF document
            for (int i = 0; i < (loadedDocument.Pages.Count); i++)
            {
                PdfPageBase page = loadedDocument.Pages[i];
                Console.WriteLine("width " + page.Size.Width + "\n hight " + page.Size.Height);
                // Extract text from the first page with bounds
                page.ExtractText(out TextLineCollection lineCollection);
                pageToSend = lineCollection;
                Console.WriteLine("width " + page.Size.Width + "\n hight " + page.Size.Height);
                PdfGraphics graphics = page.Graphics;

                //under armor
                // well done , no Notes!!
                //int height = 17;
                //string BrandNameBounds = "under armor";
                //RectangleF invoiceBounds = new RectangleF(485, 24, 60, 10);
                //RectangleF costumerIdeBounds = new RectangleF(485, 35, 75, 10);
                //RectangleF billToBounds = new RectangleF(68, 190, 120, 35);
                //RectangleF shipToBounds = new RectangleF(360, 190, 150, 60);
                //RectangleF shipDateBounds = new RectangleF(306, 298, 143, 15);
                //RectangleF orderBounds = new RectangleF(450, 270, 144, 12);
                //RectangleF qtyBounds = new RectangleF(21, 344, 30, height);
                //RectangleF productsBounds = new RectangleF(162, 344, 80, height);
                //RectangleF priceBounds = new RectangleF(404, 344, 42, height);
                //RectangleF discountAmountBounds = new RectangleF(450, 344, 35, height);
                //RectangleF totalNetPriceBounds = new RectangleF(533, 579, 60, 12);

                //under armor (Old 2019)
                //the Discount percentage is different , providing wrong details
                //int height = 38;
                //string BrandNameBounds = "under armor Old";
                //RectangleF invoiceBounds = new RectangleF(432, 58, 70, 10);
                //RectangleF costumerIdeBounds = new RectangleF(50, 305, 110, 10);
                //RectangleF billToBounds = new RectangleF(100, 137, 160, 65);
                //RectangleF shipToBounds = new RectangleF(405, 205, 160, 65);
                //RectangleF shipDateBounds = new RectangleF(330, 330, 83, 13);
                //RectangleF orderBounds = new RectangleF(432, 84, 70, 12);
                //RectangleF qtyBounds = new RectangleF(290, 372, 5, height);
                //RectangleF productsBounds = new RectangleF(8, 372, 76, height);
                //RectangleF priceBounds = new RectangleF(320, 372, 30, height);
                //RectangleF discountAmountBounds = new RectangleF(383, 372, 35, height); ;
                //RectangleF totalNetPriceBounds = new RectangleF(0, 0, 0, 0);

                //skechers 
                //well done ,no mistakes
                int height = 35;
                string BrandNameBounds = "skechers";
                RectangleF invoiceBounds = new RectangleF(450, 130, 100, 13);
                RectangleF costumerIdeBounds = new RectangleF(15, 240, 50, 15);
                RectangleF billToBounds = new RectangleF(66, 152, 225, 62);
                RectangleF shipToBounds = new RectangleF(330, 150, 225, 62);
                RectangleF shipDateBounds = new RectangleF(339, 130, 106, 13);
                RectangleF orderBounds = new RectangleF(208, 270, 105, 13);
                RectangleF qtyBounds = new RectangleF(406, 350, 30, height);
                RectangleF productsBounds = new RectangleF(12, 350, 48, height);
                RectangleF priceBounds = new RectangleF(440, 350, 55, height);
                RectangleF discountAmountBounds = new RectangleF(436, 780, 22, 16);
                RectangleF totalNetPriceBounds = new RectangleF(503, 761, 87, 10);

                //Taylor made
                //well done, no notes
                //int height = 53;
                //string BrandNameBounds = "Taylor made";
                //RectangleF invoiceBounds = new RectangleF(260, 106, 60, 15);
                //RectangleF costumerIdeBounds = new RectangleF(120, 140, 60, 10);
                //RectangleF billToBounds = new RectangleF(36, 150, 190, 85);
                //RectangleF shipToBounds = new RectangleF(305, 150, 215, 85);
                //RectangleF shipDateBounds = new RectangleF(335, 275, 60, 23);
                //RectangleF orderBounds = new RectangleF(36, 275, 50, 20);
                //RectangleF qtyBounds = new RectangleF(188, 335, 25, height);
                //RectangleF productsBounds = new RectangleF(35, 335, 40, height);
                //RectangleF priceBounds = new RectangleF(300, 335, 46, height);
                //RectangleF discountAmountBounds = new RectangleF(345, 335, 40, height);
                //RectangleF totalNetPriceBounds = new RectangleF(290, 637, 69, 20);

                //Callaway
                //works fine , no notes
                //string BrandNameBounds = "Callaway";
                //int height = 30;
                //RectangleF invoiceBounds = new RectangleF(21, 219, 60, 10); //
                //RectangleF costumerIdeBounds = new RectangleF(21, 244, 65, 10); //
                //RectangleF billToBounds = new RectangleF(51, 125, 182, 80);
                //RectangleF shipToBounds = new RectangleF(50, 30, 185, 83);
                //RectangleF shipDateBounds = new RectangleF(171, 220, 50, 10);
                //RectangleF orderBounds = new RectangleF(147, 244, 67, 10);
                //RectangleF qtyBounds = new RectangleF(330, 300, 25, height);
                //RectangleF productsBounds = new RectangleF(16, 300, 25, height);
                //RectangleF priceBounds = new RectangleF(390, 300, 40, height);
                //RectangleF discountAmountBounds = new RectangleF(435, 300, 25, height);
                //RectangleF totalNetPriceBounds = new RectangleF(516, 648, 54, 12);

                //Pawakaddy
                //need more samples with multiple orders and could not figureout where the discount location is
                //int height = 12;
                //string BrandNameBounds = "Pawakaddy";

                //float stratPosX = lineCollection.TextLine[9].Bounds.Location.X;
                //float stratPosY = lineCollection.TextLine[9].Bounds.Location.Y;
                //RectangleF invoiceBounds = new RectangleF(107 + stratPosX, 16 + stratPosY, 57, 10);
                //RectangleF costumerIdeBounds = new RectangleF(107 + stratPosX, 53 + stratPosY, 53, 10);
                //RectangleF billToBounds = new RectangleF(20 + stratPosX, 120 + stratPosY, 155, 65);
                //RectangleF shipToBounds = new RectangleF(244 + stratPosX, 120 + stratPosY, 155, 65);
                //RectangleF shipDateBounds = new RectangleF(111 + stratPosX, 30 + stratPosY, 46, 10);
                //RectangleF orderBounds = new RectangleF(300 + stratPosX, 52 + stratPosY, 60, 12);
                //RectangleF qtyBounds = new RectangleF(385 + stratPosX, 220 + stratPosY, 27, height);
                //RectangleF productsBounds = new RectangleF(-9 + stratPosX, 220 + stratPosY, 77, height);
                //RectangleF priceBounds = new RectangleF(424 + stratPosX, 220 + stratPosY, 35, height);
                //RectangleF discountAmountBounds = new RectangleF(522 + stratPosX, 220 + stratPosY, 65, height);
                //RectangleF totalNetPriceBounds = new RectangleF(513 + stratPosX, 563 + stratPosY, 50, 15);

                //Mizuno
                //works fine, no notes
                //int height = 50;
                //string BrandNameBounds = "Mizuno";
                //RectangleF invoiceBounds = new RectangleF(522, 113, 66, 20);
                //RectangleF costumerIdeBounds = new RectangleF(77, 124, 42, 12);
                //RectangleF billToBounds = new RectangleF(18, 137, 177, 91);
                //RectangleF shipToBounds = new RectangleF(267, 137, 182, 94);
                //RectangleF shipDateBounds = new RectangleF(520, 196, 64, 16);
                //RectangleF orderBounds = new RectangleF(70, 304, 42, 10);
                //RectangleF qtyBounds = new RectangleF(430, 294, 28, height);
                //RectangleF productsBounds = new RectangleF(12, 294, 55, height);
                //RectangleF priceBounds = new RectangleF(465, 294, 40, height);
                //RectangleF discountAmountBounds = new RectangleF(510, 294, 20, height);
                //RectangleF totalNetPriceBounds = new RectangleF(378, 685, 45, 13);

                //Cobra Puma
                //works fine, no notes
                //int height = 10;
                //string BrandNameBounds = "Cobra Puma";
                //RectangleF invoiceBounds = new RectangleF(27, 302, 136, 13);
                //RectangleF costumerIdeBounds = new RectangleF(27, 270, 136, 13);
                //RectangleF billToBounds = new RectangleF(84, 105, 180, 93);
                //RectangleF shipToBounds = new RectangleF(386, 164, 149, 81);
                //RectangleF shipDateBounds = new RectangleF(178, 270, 118, 14);
                //RectangleF orderBounds = new RectangleF(311, 270, 133, 13);
                //RectangleF qtyBounds = new RectangleF(310, 338, 28, height);
                //RectangleF productsBounds = new RectangleF(25, 338, 55, height);
                //RectangleF priceBounds = new RectangleF(343, 338, 54, height);
                //RectangleF discountAmountBounds = new RectangleF(450, 338, 58, height);
                //RectangleF totalNetPriceBounds = new RectangleF(309, 680, 90, 14);

                //Titleist
                //works fine , no notes
                //int height = 27;
                //string BrandNameBounds = "Titleist";
                //RectangleF invoiceBounds = new RectangleF(200, 225, 60, 10);
                //RectangleF costumerIdeBounds = new RectangleF(320, 225, 41, 10);
                //RectangleF billToBounds = new RectangleF(60, 115, 183, 76);
                //RectangleF shipToBounds = new RectangleF(330, 115, 175, 76);
                //RectangleF shipDateBounds = new RectangleF(495, 225, 53, 10);
                //RectangleF orderBounds = new RectangleF(440, 225, 56, 10);
                //RectangleF qtyBounds = new RectangleF(415, 307, 23, height);
                //RectangleF productsBounds = new RectangleF(5, 307, 68, height);
                //RectangleF priceBounds = new RectangleF(442, 307, 41, height);
                //RectangleF discountAmountBounds = new RectangleF(485, 307, 25, height);
                //RectangleF totalNetPriceBounds = new RectangleF(513, 675, 56, 14);

                //MOTOCADDY // reading verticaly need fixing
                //could not find discount column
                //int height = 25;
                //string BrandNameBounds = "Motocaddy";
                //RectangleF invoiceBounds = new RectangleF(430, 89, 52, 12);
                //RectangleF costumerIdeBounds = new RectangleF(427, 110, 43, 13);
                //RectangleF billToBounds = new RectangleF(46, 179, 126, 70);
                //RectangleF shipToBounds = new RectangleF(50, 273, 125, 68);
                //RectangleF shipDateBounds = new RectangleF(428, 180, 53, 14);
                //RectangleF orderBounds = new RectangleF(495, 208, 56, 11);
                //RectangleF qtyBounds = new RectangleF(63, 355, 36, height);
                //RectangleF productsBounds = new RectangleF(476, 355, 52, height);
                //RectangleF priceBounds = new RectangleF(297, 355, 32, height);
                //RectangleF discountAmountBounds = new RectangleF(0, 0, 0, 0);
                //RectangleF totalNetPriceBounds = new RectangleF(520, 398, 41, 11);

                //MASTERS217
                // could not find discount column
                //int height = 12;
                //RectangleF invoiceBounds = new RectangleF(250, 18, 75, 20);
                //RectangleF costumerIdeBounds = new RectangleF(276, 65, 56, 16);
                //RectangleF billToBounds = new RectangleF(8, 39, 164, 77);
                //RectangleF shipToBounds = new RectangleF(5, 135, 170, 75);
                //RectangleF shipDateBounds = new RectangleF(275, 76, 72, 17);
                //RectangleF orderBounds = new RectangleF(276, 93, 80, 14);
                //RectangleF qtyBounds = new RectangleF(383, 241, 44, height);
                //RectangleF productsBounds = new RectangleF(5, 241, 63, height);
                //RectangleF priceBounds = new RectangleF(469, 241, 36, height);
                //RectangleF netPriceBounds = new RectangleF(528, 233, 41, 524);
                //RectangleF totalNetPriceBounds = new RectangleF(515, 767, 53, 18);

                //ClevelandGolf
                //works fine , but better to have more examples with multiple orders
                //int height = 35;
                //string BrandNameBounds = "ClevelandGolf";
                //RectangleF invoiceBounds = new RectangleF(486, 311, 52, 17);
                //RectangleF costumerIdeBounds = new RectangleF(24, 354, 45, 10);
                //RectangleF billToBounds = new RectangleF(35, 156, 195, 84);
                //RectangleF shipToBounds = new RectangleF(368, 154, 195, 87);
                //RectangleF shipDateBounds = new RectangleF(416, 311, 49, 16);
                //RectangleF orderBounds = new RectangleF(400, 263, 33, 12);
                //RectangleF qtyBounds = new RectangleF(248, 398, 42, height);
                //RectangleF productsBounds = new RectangleF(28, 398, 64, height);
                //RectangleF priceBounds = new RectangleF(297, 398, 33, height);
                //RectangleF discountAmountBounds = new RectangleF(340, 398, 40, height);
                //RectangleF totalNetPriceBounds = new RectangleF(524, 686, 50, 11);

                //var boxItems = lineCollection.TextLine.SelectMany(x => x.WordCollection).Where(x => IsIntersected(x.Bounds, productsBounds)).ToList();
                //Console.WriteLine("box items" + boxItems);

                //Get the text provided in the bounds

                brandName = BrandNameBounds;
                bool qty = false, prod = false, price = false, changed = false;
                foreach (var txtLine in lineCollection.TextLine)
                {
                    //DrawRectangle(graphics, txtLine.Bounds, Color.Orange);
                    foreach (TextWord word in txtLine.WordCollection)
                    {
                        if (IsIntersected(qtyBounds, word.Bounds))
                            if (word.Text.Length > 0 && float.TryParse(word.Text, out float qnty))
                            {
                                qty = true;
                            }
                        if (IsIntersected(productsBounds, word.Bounds))
                            if (word.Text.Length > 0)
                            {
                                prod = true;
                            }
                        if (IsIntersected(priceBounds, word.Bounds) && float.TryParse(word.Text, out float prc))
                            if (word.Text.Length > 0)
                            {
                                price = true;
                            }
                    }
                    if (qty && prod && price)
                    {
                        //quantities.Add(qty);
                        //products.Add(prod);
                        //prices.Add(price);
                        if (brandName != "Callaway")
                        {
                            qtyBounds.Height += height;
                            productsBounds.Height += height;
                            priceBounds.Height += height;
                            if (brandName != "skechers")
                            {
                                discountAmountBounds.Height += height;
                            }
                        }
                        else
                        {
                            qtyRectangles.Add(qtyBounds);
                            priceRectangles.Add(priceBounds);
                            discountRectangles.Add(discountAmountBounds);
                            productsRectangles.Add(productsBounds);
                        }

                        if (brandName == "Callaway")
                        {
                            RectangleF tempoqty = new RectangleF(qtyBounds.X, qtyBounds.Y + height, qtyBounds.Width, qtyBounds.Height);
                            RectangleF tempoprice = new RectangleF(priceBounds.X, priceBounds.Y + height, priceBounds.Width, priceBounds.Height);
                            if (brandName != "Motocaddy")
                            {
                                RectangleF tempodiscount = new RectangleF(discountAmountBounds.X, discountAmountBounds.Y + height, discountAmountBounds.Width, discountAmountBounds.Height);
                                discountRectangles.Add(tempodiscount);

                            }
                            RectangleF tempoproduct = new RectangleF(productsBounds.X, productsBounds.Y + height, productsBounds.Width, productsBounds.Height);
                            qtyRectangles.Add(tempoqty);
                            priceRectangles.Add(tempoprice);
                            productsRectangles.Add(tempoproduct);

                        }
                        //DrawRectangle(graphics, qtyBounds, Color.Orange);
                        //DrawRectangle(graphics, productsBounds, Color.Olive);
                        //DrawRectangle(graphics, priceBounds, Color.Purple);

                        qty = false; prod = false; price = false; changed = true;
                    }
                }

                if (!changed)
                {
                    qtyBounds = new RectangleF(0, 0, 0, 0);
                    productsBounds = new RectangleF(0, 0, 0, 0);
                    priceBounds = new RectangleF(0, 0, 0, 0);
                    if (brandName != "skechers")
                    {
                        discountAmountBounds = new RectangleF(0, 0, 0, 0);
                    }
                }
                else
                {
                    if (brandName != "Callaway")
                    {
                        qtyBounds.Height -= height;
                        productsBounds.Height -= height;
                        priceBounds.Height -= height;
                        if (brandName != "skechers")
                        {
                            discountAmountBounds.Height -= height;
                        }

                    }

                }

                //ClevelandGolf => order ID
                //RectangleF orderBounds = new RectangleF(32, productsBounds.Y + productsBounds.Height+54, 50, 18);

                if (i == 0)
                {
                    DrawRectangle(graphics, invoiceBounds, Color.Red);
                    DrawRectangle(graphics, costumerIdeBounds, Color.Blue);
                    DrawRectangle(graphics, billToBounds, Color.Green);
                    DrawRectangle(graphics, shipToBounds, Color.Gold);
                    DrawRectangle(graphics, shipDateBounds, Color.GreenYellow);
                    DrawRectangle(graphics, orderBounds, Color.HotPink);
                    DrawRectangle(graphics, discountAmountBounds, Color.Black);
                    DrawRectangle(graphics, totalNetPriceBounds, Color.Black);
                }

                DrawRectangle(graphics, qtyBounds, Color.Orange);
                DrawRectangle(graphics, productsBounds, Color.Olive);
                DrawRectangle(graphics, priceBounds, Color.Purple);
                if (brandName == "Callaway")
                {
                    for (int r = 0; r < qtyRectangles.Count; r++)
                    {
                        DrawRectangle(graphics, qtyRectangles[r], Color.Purple);
                        DrawRectangle(graphics, priceRectangles[r], Color.Purple);
                        DrawRectangle(graphics, discountRectangles[r], Color.Purple);
                        DrawRectangle(graphics, productsRectangles[r], Color.Purple);
                        List<string> tempList = ExtractTextFromRectangle(pageToSend, qtyRectangles[r], priceRectangles[r], discountRectangles[r], productsRectangles[r]);
                        quantities.Add(tempList[0]);
                        prices.Add(tempList[1]);
                        discountAmount.Add(tempList[2]);
                        products.Add(tempList[3]);
                    }

                }
                //string qtyS = null, prodS = null, priceS = null;
                foreach (var txtLine in lineCollection.TextLine)
                {
                    foreach (TextWord word in txtLine.WordCollection)
                    {
                        if (i == 0)
                        {
                            if (IsIntersected(invoiceBounds, word.Bounds))
                                invoiceNumer += word.Text;
                            if (IsIntersected(costumerIdeBounds, word.Bounds))
                                costumerId += word.Text;
                            if (IsIntersected(billToBounds, word.Bounds))
                                billTo += word.Text;
                            if (IsIntersected(shipToBounds, word.Bounds))
                                shipTo += word.Text;
                            if (IsIntersected(shipDateBounds, word.Bounds))
                                shipDate += word.Text;
                            if (IsIntersected(orderBounds, word.Bounds))
                                order += word.Text;
                        }
                        if (brandName != "Callaway")
                        {

                            if (IsIntersected(qtyBounds, word.Bounds))
                            {
                                if (!string.IsNullOrWhiteSpace(word.Text))
                                {
                                    quantities.Add(word.Text);
                                }

                            }
                            if (IsIntersected(productsBounds, word.Bounds))
                            {
                                if (!string.IsNullOrWhiteSpace(word.Text))
                                {
                                    products.Add(word.Text);
                                }
                            }
                            if (IsIntersected(priceBounds, word.Bounds))
                            {
                                if (!string.IsNullOrWhiteSpace(word.Text))
                                {
                                    prices.Add(word.Text);
                                }
                            }
                            if (IsIntersected(discountAmountBounds, word.Bounds))
                            {
                                if (!string.IsNullOrWhiteSpace(word.Text))
                                {
                                    discountAmount.Add(word.Text);
                                }
                            }
                            if (IsIntersected(totalNetPriceBounds, word.Bounds))
                                totalNetPrice += word.Text;

                        }


                    }
                    //the Old One That Works For All Other Templates
                    //foreach (TextWord word in txtLine.WordCollection)
                    //{
                    //    if (i == 0)
                    //    {
                    //        if (IsIntersected(invoiceBounds, word.Bounds))
                    //            invoiceNumer += word.Text;
                    //        if (IsIntersected(costumerIdeBounds, word.Bounds))
                    //            costumerId += word.Text;
                    //        if (IsIntersected(billToBounds, word.Bounds))
                    //            billTo += word.Text;
                    //        if (IsIntersected(shipToBounds, word.Bounds))
                    //            shipTo += word.Text;
                    //        if (IsIntersected(shipDateBounds, word.Bounds))
                    //            shipDate += word.Text;
                    //        if (IsIntersected(orderBounds, word.Bounds))
                    //            order += word.Text;
                    //    }

                    //    if (IsIntersected(qtyBounds, word.Bounds))
                    //    {
                    //        if (!string.IsNullOrWhiteSpace(word.Text))
                    //        {
                    //            temp += word.Text;
                    //            quantities.Add(word.Text);
                    //        }

                    //    }
                    //    if (IsIntersected(productsBounds, word.Bounds))
                    //    {
                    //        if (!string.IsNullOrWhiteSpace(word.Text))
                    //        {
                    //            products.Add(word.Text);
                    //        }
                    //    }
                    //    if (IsIntersected(priceBounds, word.Bounds))
                    //    {
                    //        if (!string.IsNullOrWhiteSpace(word.Text))
                    //        {
                    //            prices.Add(word.Text);
                    //        }
                    //    }
                    //    if (IsIntersected(discountAmountBounds, word.Bounds))
                    //    {
                    //        if (!string.IsNullOrWhiteSpace(word.Text))
                    //        {
                    //            discountAmount.Add(word.Text);
                    //        }
                    //    }
                    //    if (IsIntersected(totalNetPriceBounds, word.Bounds))
                    //        totalNetPrice += word.Text;


                    //}
                    //if (qtyS != null && prodS != null && priceS != null)
                    //{
                    //    quantities.Add(qtyS);
                    //    products.Add(prodS);
                    //    prices.Add(priceS);

                    //    qtyS = null; prodS = null; priceS = null;
                    //}
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
            foreach (var discount in discountAmount)
                data += "\n\n-" + discount;
            Console.WriteLine(data);
            Validate(quantities, prices, discountAmount, totalNetPrice, brandName);

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
        private void Validate(List<string> QTY, List<string> productPrice, List<string> discountAmount, string totalNet, string brand)
        {
            float total = 0;
            if (QTY.LongCount() > 0)
            {

                for (int i = 0; i < QTY.LongCount(); i++)
                {
                    if (!string.IsNullOrWhiteSpace(QTY[i]) && QTY[i] != "")
                    {

                        if (brand == "under armor Old")
                        {
                            total += (float.Parse(productPrice[i].Trim()) * (1 - (float.Parse(discountAmount[i].Trim()) / 100)));
                        }
                        else
                        {
                            if (discountAmount.Count ==1)
                            {
                                if (discountAmount[0].EndsWith("%"))
                                {
                                    total += (
                                 int.Parse(QTY[i].Trim())
                                 * float.Parse(productPrice[i].Trim())
                                 * (1 - (float.Parse(discountAmount[0].TrimEnd('%')) / 100)));
                                }
                                else
                                {
                                    total += (
                                  int.Parse(QTY[i].Trim())
                                  * float.Parse(productPrice[i].Trim())
                                  * (1 - (float.Parse(discountAmount[i].Trim()) / 100)));

                                }
                            }
                            else
                            {
                                if (discountAmount[i].EndsWith("%"))
                                {
                                    total += (
                                 int.Parse(QTY[i].Trim())
                                 * float.Parse(productPrice[i].Trim())
                                 * (1 - (float.Parse(discountAmount[i].TrimEnd('%')) / 100)));
                                }
                                else
                                {
                                    total += (
                                  int.Parse(QTY[i].Trim())
                                  * float.Parse(productPrice[i].Trim())
                                  * (1 - (float.Parse(discountAmount[i].Trim()) / 100)));

                                }
                            }
                        

                        }

                    }
                }
                //if (float.Parse(totalNet.Trim()) == total)
                //{
                //    Console.WriteLine("\n\n VALID \n\n");
                //}
                //else
                //{
                //    Console.WriteLine("\n\nNOT ACCEPTABLE\n\n");
                //}
            }
            Console.WriteLine("TOTAL IS EQUAL TO :" + total);

        }


        public List<string> ExtractTextFromRectangle(TextLineCollection page, RectangleF qtyRec, RectangleF priceRec, RectangleF discountRec, RectangleF productRec)
        {
            string qty = "";
            string price = "";
            string discount = "";
            string product = "";
            foreach (var txtLine in page.TextLine)
            {
                foreach (TextWord word in txtLine.WordCollection)
                {
                    if (IsIntersected(qtyRec, word.Bounds))
                    {
                        qty += word.Text;
                    }
                    if (IsIntersected(priceRec, word.Bounds))
                    {
                        price += word.Text;
                    }
                    if (IsIntersected(discountRec, word.Bounds))
                    {
                        discount += word.Text;
                    }
                    if (IsIntersected(productRec, word.Bounds))
                    {
                        product += word.Text;
                    }
                }
            }

            return new List<string> { qty, price, discount, product };
        }

    }

}
