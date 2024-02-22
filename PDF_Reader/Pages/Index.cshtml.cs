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
using PDF_Reader.Models;
using PDF_Reader.BrandtracProcessor;
using System.Diagnostics;


namespace PDF_Reader.Pages
{
    public class IndexModel : PageModel
    {
        Order order1 = new();
        private string invoiceNumer = "";
        private string costumerId = "";
        private string billTo = "";
        private string shipTo = "";
        private string shipDate = "";
        private string order = "";
        private int totalQuantity = 0;
        private List<string> quantities = new List<string>();
        private List<string> products = new List<string>();
        private List<string> barCodes = new List<string>();
        private List<string> prices = new List<string>();
        private List<string> discountAmount = new List<string>();
        private string totalNetPrice = "";
        static int height = 10;
        RectangleF invoiceBounds = new RectangleF(27, 302, 136, 13);
        RectangleF costumerIdeBounds = new RectangleF(27, 270, 136, 13);
        RectangleF billToBounds = new RectangleF(84, 105, 180, 93);
        RectangleF shipToBounds = new RectangleF(386, 164, 149, 81);
        RectangleF shipDateBounds = new RectangleF(178, 270, 118, 14);
        RectangleF orderBounds = new RectangleF(311, 270, 133, 13);
        RectangleF qtyBounds = new RectangleF(310, 338, 28, height);
        RectangleF productsBounds = new RectangleF(25, 338, 55, height);
        RectangleF priceBounds = new RectangleF(343, 338, 54, height);
        RectangleF discountAmountBounds = new RectangleF(450, 338, 58, height);
        RectangleF totalNetPriceBounds = new RectangleF(309, 680, 90, 14);


        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostPdfText(IFormFile fileUpload)
        {
            string fileName = "";
            if (fileUpload != null && fileUpload.Length > 0)
            {
                fileName = fileUpload.FileName;
                using (Stream stream = fileUpload.OpenReadStream())
                {
                    await Button_Click(stream, fileName);
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
                    //return File(new FileStream($"{fileName}-modified.pdf", FileMode.Open), "application/pdf", $"{fileName}-modified.pdf");
                }
            }
            return null;
        }

        public async Task Button_Click(Stream file, string filename)
        {
            Order order1 = new Order();

            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(file);


            string brandName = await GetBrandName(file);


            if (brandName == "UA")
            {
                UnderArmorProcessor processor = new UnderArmorProcessor();
                await processor.ExtractData(file, filename);
            }
            else if (brandName == "Callaway")
            {
                CallawayProcessor processor = new CallawayProcessor();
                await processor.ExtractData(file, filename);
            }
            else if (brandName == "Skechers")
            {
                SkechersProccessor processor = new SkechersProccessor();
                await processor.ExtractData(file, filename);
            }
            else if (brandName == "TaylorMade")
            {
                TaylorMadeProcessor processor = new TaylorMadeProcessor();
                await processor.ExtractData(file, filename);
            }
            else if (brandName == "Powakaddy")
            {
                PowakaddyProcessor processor = new PowakaddyProcessor();
                await processor.ExtractData(file, filename);
            }
            else if (brandName == "Motocaddy")
            {
                MotocaddyProcessor processor = new MotocaddyProcessor();
                await processor.ExtractData(file, filename);
            }
            else if (brandName == "Mizuno")
            {
                MizunoProcessor processor = new MizunoProcessor();
                await processor.ExtractData(file, filename);
            }
            else if (brandName == "CobraPuma")
            {
                CobraPumaProcessor processor = new CobraPumaProcessor();
                await processor.ExtractData(file, filename);
            }
            else if (brandName == "Titleist")
            {
                TitleistProcessor processor = new TitleistProcessor();
                await processor.ExtractData(file, filename);
            }
            else if (brandName == "Masters217")
            {
                Masters217Processors processor = new Masters217Processors();
                await processor.ExtractData(file, filename);
            }
            else if (brandName == "ClevelandGolf")
            {
                ClevelandProcessor processor = new ClevelandProcessor();
                await processor.ExtractData(file, filename);
            }
            else if (brandName == "UAOld")
            {
                UnderAromorOldProcessor processor = new UnderAromorOldProcessor();
                await processor.ExtractData(file, filename);
            }
            else if (brandName == "Adidas")
            {
                AdidasProcessor processor = new AdidasProcessor();
                await processor.ExtractData(file, filename);
            }
            else if (brandName == "other")
            {
                //transfate the file to a file to get check manualy later
                //await TransferBlobAsync(fileName);
            }
            //Stream[] extractedImages = loadedDocument.Pages[0].ExtractImages();


            //using (FileStream outputFileStream = new FileStream($"{filename}-modified.pdf", FileMode.Create))
            //{
            //    loadedDocument.Save(outputFileStream);
            //}
            //loadedDocument.Close(true);

            //string data = "";

            //data = "Invoice Number: " + invoiceNumer;
            //data += "\n\nCostumer ID: " + costumerId;
            //data += "\n\nBill To: " + billTo;
            //data += "\n\nShip To: " + shipTo;
            //data += "\n\nShip Date: " + shipDate;
            //data += "\n\nOrder: " + order;
            //data += "\n\nTotal Net Price: " + totalNetPrice;
            //data += "\n\n------------\nQTY:";
            //foreach (var qty in quantities)
            //    data += "\n\n" + qty;
            //data += "\n\n------------\nProducts:";
            //foreach (var p in products)
            //    data += "\n\n" + p;
            //data += "\n\n------------\nPrices:";
            //foreach (var p in prices)
            //    data += "\n\n" + p;
            //data += "\n\n------------\nNet Prices:";
            //foreach (var discount in discountAmount)
            //    data += "\n\n-" + discount;
            //Console.WriteLine(data);
            //Validate(quantities, prices, discountAmount, totalNetPrice, brandName);



        }
        public async void Puma(PdfLoadedDocument loadedDocument, int i)
        {

            string invoiceNumer = "";
            string costumerId = "";
            string billTo = "";
            string shipTo = "";
            string shipDate = "";
            string order = "";
            int totalQuantity = 0;
            var quantities = new List<string>();
            var products = new List<string>();
            var prices = new List<string>();
            var discountAmount = new List<string>();
            string totalNetPrice = "";

            PdfPageBase page = loadedDocument.Pages[i];
            // Extract text from the first page with bounds
            page.ExtractText(out TextLineCollection lineCollection);
            //pageToSend = lineCollection;
            PdfGraphics graphics = page.Graphics;

            int height = 10;
            RectangleF invoiceBounds = new RectangleF(27, 302, 136, 13);
            RectangleF costumerIdeBounds = new RectangleF(27, 270, 136, 13);
            RectangleF billToBounds = new RectangleF(84, 105, 180, 93);
            RectangleF shipToBounds = new RectangleF(386, 164, 149, 81);
            RectangleF shipDateBounds = new RectangleF(178, 270, 118, 14);
            RectangleF orderBounds = new RectangleF(311, 270, 133, 13);
            RectangleF qtyBounds = new RectangleF(310, 338, 28, height);
            RectangleF productsBounds = new RectangleF(25, 338, 55, height);
            RectangleF priceBounds = new RectangleF(343, 338, 54, height);
            RectangleF discountAmountBounds = new RectangleF(450, 338, 58, height);
            RectangleF totalNetPriceBounds = new RectangleF(309, 680, 90, 14);



            bool qty = false, prod = false, price = false, changed = false;
            foreach (var txtLine in lineCollection.TextLine)
            {
                //DrawRectangle(graphics, txtLine.Bounds, Color.Orange);
                foreach (TextWord word in txtLine.WordCollection)
                {
                    if (IsIntersected(qtyBounds, word.Bounds) || qty)
                        if ((word.Text.Length > 0 && float.TryParse(word.Text, out float qnty)) || qty)
                        {
                            if (word.Bounds.Y - qtyBounds.Y > 50)
                            {
                                qty = false;
                            }
                            else
                            {
                                qty = true;
                                qtyBounds.Height = (word.Bounds.Y - qtyBounds.Y + word.Bounds.Height + 2);
                            }
                        }
                    if (IsIntersected(productsBounds, word.Bounds) || prod)
                        if (word.Text.Length > 0 || prod)
                        {
                            if (word.Bounds.Y - qtyBounds.Y > 50)
                            {
                                prod = false;
                            }
                            else
                            {
                                prod = true;
                                productsBounds.Height = (word.Bounds.Y - productsBounds.Y + word.Bounds.Height + 2);
                            }
                        }
                    if (IsIntersected(priceBounds, word.Bounds) && float.TryParse(word.Text, out float prc) || price)
                        if (word.Text.Length > 0 || price)
                        {
                            if (word.Bounds.Y - qtyBounds.Y > 50)
                            {
                                price = false;
                            }
                            else
                            {
                                price = true;
                                priceBounds.Height = (word.Bounds.Y - priceBounds.Y + word.Bounds.Height + 2);
                            }
                        }
                }
                if (qtyBounds.Height == productsBounds.Height && productsBounds.Height == priceBounds.Height)
                {

                    totalNetPriceBounds = new RectangleF(526, productsBounds.Y + productsBounds.Height + 37, 30, 10);
                }
            }
            DrawRectangle(graphics, qtyBounds, Color.Orange);
            DrawRectangle(graphics, productsBounds, Color.Olive);
            DrawRectangle(graphics, priceBounds, Color.Purple);

            DrawRectangle(graphics, invoiceBounds, Color.Red);
            DrawRectangle(graphics, costumerIdeBounds, Color.Blue);
            DrawRectangle(graphics, billToBounds, Color.Green);
            DrawRectangle(graphics, shipToBounds, Color.Gold);
            DrawRectangle(graphics, shipDateBounds, Color.GreenYellow);
            DrawRectangle(graphics, orderBounds, Color.HotPink);
            DrawRectangle(graphics, discountAmountBounds, Color.Black);
            DrawRectangle(graphics, totalNetPriceBounds, Color.Black);


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



                    if (IsIntersected(qtyBounds, word.Bounds))
                    {
                        if (!string.IsNullOrWhiteSpace(word.Text))
                        {
                            quantities.Add(word.Text);
                            totalQuantity += int.Parse(word.Text);
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


            //Validate(quantities, prices, discountAmount, totalNetPrice, "Motocaddy");
        }

        public async void Motocaddy(PdfLoadedDocument loadedDocument, int i)
        {
            string invoiceNumer = "";
            string costumerId = "";
            string billTo = "";
            string shipTo = "";
            string shipDate = "";
            string order = "";
            int totalQuantity = 0;
            var quantities = new List<string>();
            var products = new List<string>();
            var prices = new List<string>();
            var discountAmount = new List<string>();
            string totalNetPrice = "";

            PdfPageBase page = loadedDocument.Pages[i];
            // Extract text from the first page with bounds
            page.ExtractText(out TextLineCollection lineCollection);
            //pageToSend = lineCollection;
            PdfGraphics graphics = page.Graphics;

            int height = 10;

            RectangleF invoiceBounds = new RectangleF(430, 89, 52, 12);
            RectangleF costumerIdeBounds = new RectangleF(427, 110, 43, 13);
            RectangleF billToBounds = new RectangleF(46, 179, 126, 70);
            RectangleF shipToBounds = new RectangleF(50, 273, 125, 68);
            RectangleF shipDateBounds = new RectangleF(428, 180, 53, 14);
            RectangleF orderBounds = new RectangleF(495, 208, 56, 11);
            RectangleF qtyBounds = new RectangleF(63, 355, 36, height);
            RectangleF productsBounds = new RectangleF(476, 355, 52, height);
            RectangleF priceBounds = new RectangleF(297, 355, 32, height);
            RectangleF discountAmountBounds = new RectangleF(0, 0, 0, 0);
            RectangleF totalNetPriceBounds = new RectangleF(0, 0, 0, 0);




            bool qty = false, prod = false, price = false, changed = false;
            foreach (var txtLine in lineCollection.TextLine)
            {
                //DrawRectangle(graphics, txtLine.Bounds, Color.Orange);
                foreach (TextWord word in txtLine.WordCollection)
                {
                    if (IsIntersected(qtyBounds, word.Bounds) || qty)
                        if ((word.Text.Length > 0 && float.TryParse(word.Text, out float qnty)) || qty)
                        {
                            if (qtyBounds.Y >= word.Bounds.Y)
                            {
                                qty = false;
                            }
                            else
                            {
                                qty = true;
                                qtyBounds.Height = (word.Bounds.Y - qtyBounds.Y + word.Bounds.Height + 2);
                            }
                        }
                    if (IsIntersected(productsBounds, word.Bounds) || prod)
                        if (word.Text.Length > 0 || prod)
                        {
                            if (productsBounds.Y >= word.Bounds.Y)
                            {
                                prod = false;
                            }
                            else
                            {
                                prod = true;
                                productsBounds.Height = (word.Bounds.Y - productsBounds.Y + word.Bounds.Height + 2);
                            }
                        }
                    if (IsIntersected(priceBounds, word.Bounds) && float.TryParse(word.Text, out float prc) || price)
                        if (word.Text.Length > 0 || price)
                        {
                            if (priceBounds.Y >= word.Bounds.Y)
                            {
                                price = false;
                            }
                            else
                            {
                                price = true;
                                priceBounds.Height = (word.Bounds.Y - priceBounds.Y + word.Bounds.Height + 2);
                            }
                        }
                }
                if (qtyBounds.Height == productsBounds.Height && productsBounds.Height == priceBounds.Height)
                {

                    totalNetPriceBounds = new RectangleF(526, productsBounds.Y + productsBounds.Height + 37, 30, 10);
                    DrawRectangle(graphics, qtyBounds, Color.Orange);
                    DrawRectangle(graphics, productsBounds, Color.Olive);
                    DrawRectangle(graphics, priceBounds, Color.Purple);

                    DrawRectangle(graphics, invoiceBounds, Color.Red);
                    DrawRectangle(graphics, costumerIdeBounds, Color.Blue);
                    DrawRectangle(graphics, billToBounds, Color.Green);
                    DrawRectangle(graphics, shipToBounds, Color.Gold);
                    DrawRectangle(graphics, shipDateBounds, Color.GreenYellow);
                    DrawRectangle(graphics, orderBounds, Color.HotPink);
                    DrawRectangle(graphics, discountAmountBounds, Color.Black);
                    DrawRectangle(graphics, totalNetPriceBounds, Color.Black);
                }
            }


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



                    if (IsIntersected(qtyBounds, word.Bounds))
                    {
                        if (!string.IsNullOrWhiteSpace(word.Text))
                        {
                            quantities.Add(word.Text);
                            totalQuantity += int.Parse(word.Text);
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


            Validate(quantities, prices, discountAmount, totalNetPrice, "Motocaddy");
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
                            if (discountAmount.Count == 1)
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
                                  (int)Math.Round(double.Parse(QTY[i].Trim()))
                                  * float.Parse(productPrice[i].Trim())
                                  * (1 - (float.Parse(discountAmount[i].Trim()) / 100)));

                                }
                            }
                            else if (discountAmount.Count == 0)
                            {
                                total += (
                                  (int)Math.Round(double.Parse(QTY[i].Trim()))
                                  * float.Parse(productPrice[i].Trim()));
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
                if (Math.Abs(float.Parse(totalNet.Trim()) - total) < 0.0001)
                {
                    Console.WriteLine("\n\n VALID \n\n");
                }
                else
                {
                    Console.WriteLine("\n\nNOT ACCEPTABLE\n\n");
                }
            }
            Console.WriteLine("CALCULATED TOTAL IS EQUAL TO :" + total);

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
        public List<string> ExtractTextFromRectangleForNoDiscount(TextLineCollection page, RectangleF qtyRec, RectangleF priceRec, RectangleF productRec)
        {
            string qty = "";
            string price = "";
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
                    if (IsIntersected(productRec, word.Bounds))
                    {
                        product += word.Text;
                    }
                }
            }

            return new List<string> { qty, price, product };
        }

        public async Task<string> GetBrandName(Stream file)
        {
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(file);

            PdfPageBase page = loadedDocument.Pages[0];


            // Extract text from the first page with bounds
            page.ExtractText(out TextLineCollection lineCollection);
            string name = "";
            float stratPosX = lineCollection.TextLine[9].Bounds.Location.X;
            float stratPosY = lineCollection.TextLine[9].Bounds.Location.Y;

            Console.WriteLine("width " + page.Size.Width + "\n hight " + page.Size.Height);
            foreach (var txtLine in lineCollection.TextLine)
            {
                //DrawRectangle(graphics, txtLine.Bounds, Color.Orange);
                foreach (TextWord word in txtLine.WordCollection)
                {

                    // look for Callaway
                    if (IsIntersected(new RectangleF(433, 89, 52, 10), word.Bounds))
                    {
                        name += word.Text;
                        if (name.Contains("CALLAWAY")) return "Callaway";

                    }
                    // look for Under Armor old
                    if (IsIntersected(new RectangleF(13, 70, 68, 11), word.Bounds))
                    {
                        name += word.Text;
                        if (name.Contains("Under Armour")) return "UAOld";
                    }
                    // look for Masters
                    if (IsIntersected(new RectangleF(443, 82, 32, 10), word.Bounds))
                        if (word.Text.Contains("Masters")) return "Masters217";
                    // look for Skechers
                    if (IsIntersected(new RectangleF(346, 51, 103, 10), word.Bounds))
                    {
                        name += word.Text;
                        if (name.Contains("Make payment to")) return "Skechers";
                    }
                    // look for Mizuno
                    if (IsIntersected(new RectangleF(273, 26, 52, 18), word.Bounds))
                        if (word.Text.Contains("MIZUNO")) return "Mizuno";
                    // look for Powakaddy
                    if (IsIntersected(new RectangleF(434 + stratPosX, 37 + stratPosY, 50, 10), word.Bounds))
                        if (word.Text.Contains("PowaKaddy")) return "Powakaddy";
                    // look for Under Armor
                    if (IsIntersected(new RectangleF(65, 67, 63, 11), word.Bounds))
                    {
                        name += word.Text;
                        if (name.Contains("UNDER ARMOUR")) return "UA";
                    }
                    // look for TaylorMade
                    if (IsIntersected(new RectangleF(55, 782, 40, 10), word.Bounds))
                        if (word.Text.Contains("TaylorMade")) return "TaylorMade";
                    // look for CobraPuma
                    if (IsIntersected(new RectangleF(58, 820, 74, 10), word.Bounds))
                        if (word.Text.Contains("Puma")) return "CobraPuma";
                    // look for Motocaddy
                    if (IsIntersected(new RectangleF(30, 67, 45, 10), word.Bounds))
                        if (word.Text.Contains("Motocaddy")) return "Motocaddy";
                    // look for Cleveland
                    if (IsIntersected(new RectangleF(34, 65, 45, 10), word.Bounds))
                        if (word.Text.Contains("Srixon")) return "ClevelandGolf";
                    // look for Adidas
                    if (IsIntersected(new RectangleF(678, 87, 33, 13), word.Bounds))
                        if (word.Text.Contains("adidas")) return "Adidas";
                    // look for Titleist
                    if (IsIntersected(new RectangleF(42, 795, 93, 10), word.Bounds))
                    {
                        name += word.Text;
                        if (name.Contains("Acushnet Europe Limited")) return "Titleist";
                    }
                }
            }

            return "other";

        }

    }

}
