using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using XPOS.Shared.Models;

namespace PDF_Reader.Pages
{
    public class PowakaddyProcessor : ABrandTrackProcessor, IBrandTrackProcessor
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
        private List<string> description = new List<string>();
        private List<string> discountAmount = new List<string>();
        private string totalNetPrice = "";
        private List<string> vatRate = new List<string>();


        static int height = 12;
        static float stratPosX = 0;
        static float stratPosY = 0;

        RectangleF invoiceBounds = new RectangleF(107 + stratPosX, 16 + stratPosY, 57, 10);
        RectangleF costumerIdeBounds = new RectangleF(107 + stratPosX, 53 + stratPosY, 53, 10);
        RectangleF billToBounds = new RectangleF(20 + stratPosX, 120 + stratPosY, 155, 65);
        RectangleF shipToBounds = new RectangleF(244 + stratPosX, 120 + stratPosY, 155, 65);
        RectangleF shipDateBounds = new RectangleF(111 + stratPosX, 30 + stratPosY, 46, 10);
        RectangleF orderBounds = new RectangleF(300 + stratPosX, 52 + stratPosY, 60, 12);
        RectangleF qtyBounds = new RectangleF(392 + stratPosX, 220 + stratPosY, 20, height);
        RectangleF productsBounds = new RectangleF(-9 + stratPosX, 220 + stratPosY, 77, height);
        RectangleF descriptionBounds = new RectangleF(76 + stratPosX, 220 + stratPosY, 200, height);
        RectangleF priceBounds = new RectangleF(424 + stratPosX, 220 + stratPosY, 35, height);
        RectangleF discountAmountBounds = new RectangleF(522 + stratPosX, 220 + stratPosY, 65, height);
        RectangleF totalNetPriceBounds = new RectangleF(513 + stratPosX, 602 + stratPosY, 50, 15);
        RectangleF vatBounds = new RectangleF(100 + stratPosX, 584 + stratPosY, 25, 11);


        List<RectangleF> qtyRectangles = new List<RectangleF>();
        List<RectangleF> priceRectangles = new List<RectangleF>();
        List<RectangleF> discountRectangles = new List<RectangleF>();
        List<RectangleF> productsRectangles = new List<RectangleF>();
        List<RectangleF> descriptionRectangles = new List<RectangleF>();
        List<RectangleF> vatRectangles = new List<RectangleF>();


        //private XCODESImports xCODESImports = new();
        //private MainDbContext _mainDbContext = new();
        private string shopid;

        //public PowakaddyProcessor(string shopid)
        //{
        //    this.shopid = shopid;
        //}

        public async Task CreatStockIn(string fileName)
        {
            //try
            //{
            //    _mainDbContext.Database.BeginTransaction();
            //    // add stock in to databse
            //    var newOrder = await StockController.CreateOrder(_mainDbContext, order1, shopid, staffid);

            //    // move the file to the other azure storage
            //    //await TransferBlobAsync(fileName);


            //    // delete the file from the first azure storage
            //    //await DeleteBlobAsync(fileName);

            //    await _mainDbContext.Database.CommitTransactionAsync();
            //}
            //catch (Exception ex)
            //{
            //    await _mainDbContext.Database.RollbackTransactionAsync();
            //    //throw ex;
            //    // show error massage on the screen
            //}
        }

        public async Task ExtractData(Stream file, string fileName)
        {
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(file);
            int lastPageNum = loadedDocument.Pages.Count - 1;


            // Loop in the pages of the loaded PDF document
            for (int i = 0; i < (loadedDocument.Pages.Count); i++)
            {
                PdfPageBase page = loadedDocument.Pages[i];
                //Console.WriteLine("width " + page.Size.Width + "\n hight " + page.Size.Height);
                // Extract text from the first page with bounds
                page.ExtractText(out TextLineCollection lineCollection);
                PdfGraphics graphics = page.Graphics;

                stratPosX = lineCollection.TextLine[9].Bounds.Location.X;
                stratPosY = lineCollection.TextLine[9].Bounds.Location.Y;

                invoiceBounds = new RectangleF(107 + stratPosX, 16 + stratPosY, 57, 10);
                costumerIdeBounds = new RectangleF(107 + stratPosX, 53 + stratPosY, 53, 10);
                billToBounds = new RectangleF(20 + stratPosX, 120 + stratPosY, 155, 65);
                shipToBounds = new RectangleF(244 + stratPosX, 120 + stratPosY, 155, 65);
                shipDateBounds = new RectangleF(111 + stratPosX, 30 + stratPosY, 46, 10);
                orderBounds = new RectangleF(300 + stratPosX, 52 + stratPosY, 60, 12);
                qtyBounds = new RectangleF(385 + stratPosX, 220 + stratPosY, 20, height);
                productsBounds = new RectangleF(-9 + stratPosX, 220 + stratPosY, 77, height);
                descriptionBounds = new RectangleF(76 + stratPosX, 220 + stratPosY, 200, height);
                priceBounds = new RectangleF(424 + stratPosX, 220 + stratPosY, 35, height);
                discountAmountBounds = new RectangleF(522 + stratPosX, 220 + stratPosY, 65, height);
                totalNetPriceBounds = new RectangleF(513 + stratPosX, 585 + stratPosY, 50, 15);
                vatBounds = new RectangleF(100 + stratPosX, 567 + stratPosY, 25, 11);

                foreach (var txtLine in lineCollection.TextLine)
                {
                    //DrawRectangle(graphics, txtLine.Bounds, Color.Orange);A
                    foreach (TextWord word in txtLine.WordCollection)
                    {
                        if (IsIntersected(qtyBounds, word.Bounds))
                            if (word.Text.Trim().Length > 0 && float.TryParse(word.Text, out float qnty))
                            {
                                qtyRectangles.Add(word.Bounds);
                            }
                    }
                }

                for (int j = 0; j < qtyRectangles.Count; j++)
                {
                    float newHeight = 0;
                    if (qtyRectangles.Count - 1 > j)
                    {
                        newHeight = qtyRectangles[j + 1].Y - qtyRectangles[j].Y - 3;
                    }
                    else newHeight = 10;

                    productsBounds.Y = qtyRectangles[j].Y;
                    priceBounds.Y = qtyRectangles[j].Y;
                    discountAmountBounds.Y = qtyRectangles[j].Y;
                    descriptionBounds.Y = qtyRectangles[j].Y;
                  

                    productsBounds.Height = newHeight;
                    priceBounds.Height = newHeight;
                    discountAmountBounds.Height = newHeight;
                    descriptionBounds.Height = newHeight;
                 

                    productsRectangles.Add(productsBounds);
                    priceRectangles.Add(priceBounds);
                    discountRectangles.Add(discountAmountBounds);
                    descriptionRectangles.Add(descriptionBounds);
                   


                    DrawRectangle(graphics, discountAmountBounds, Color.Black);
                    DrawRectangle(graphics, qtyRectangles[j], Color.Orange);
                    DrawRectangle(graphics, productsBounds, Color.Olive);
                    DrawRectangle(graphics, descriptionBounds, Color.Black);
                    DrawRectangle(graphics, priceBounds, Color.Purple);
                    DrawRectangle(graphics, vatBounds, Color.Purple);
                }

                DrawRectangle(graphics, invoiceBounds, Color.Red);
                DrawRectangle(graphics, costumerIdeBounds, Color.Blue);
                DrawRectangle(graphics, billToBounds, Color.Green);
                DrawRectangle(graphics, shipToBounds, Color.Gold);
                DrawRectangle(graphics, shipDateBounds, Color.GreenYellow);
                DrawRectangle(graphics, orderBounds, Color.HotPink);
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
                            if (IsIntersected(vatBounds, word.Bounds))
                                vatRate.Add(word.Text) ;
                        }

                        if (IsIntersected(totalNetPriceBounds, word.Bounds))
                            totalNetPrice += float.TryParse(word.Text.Trim(), out float v) ? v : 0;

                    }
                }

                for (int r = 0; r < qtyRectangles.Count; r++)
                {
                    List<string> tempList = ExtractTextFromRectangle(lineCollection, qtyRectangles[r], priceRectangles[r], discountRectangles[r], productsRectangles[r], descriptionRectangles[r], vatBounds);
                    quantities.Add(tempList[0]);
                    prices.Add(tempList[1]);
                    discountAmount.Add(tempList[2]);
                    products.Add(tempList[3]);
                    description.Add(tempList[4]);
                    //vatRate.Add(tempList[5]);
                    totalQuantity += (int) float.Parse(tempList[0]);
                }

                float total = 0;
                for (int j = 0; j < quantities.LongCount(); j++)
                {
                    if (!string.IsNullOrWhiteSpace(quantities[j]) && quantities[j] != "")
                    {
                        total += (
                              (int) float.Parse(quantities[j].Trim())
                              * float.Parse(prices[j].Trim())
                              * (1 - (float.Parse(discountAmount[j].Trim()) / 100)));
                    }
                }

                //if (Pricecheck(total, totalNetPrice))
                //{
                order1.OrderType = OrderType.StockIn;
                    order1.InvoiceNo = invoiceNumer;
                    order1.Process = OrderProcess.Brandtrac;
                    order1.Quantity = totalQuantity;
                    order1.SupplierID = "PowaKaddy International Limited";
                    for (int ds = 0; ds < quantities.Count; ds++)
                    {
                        //barCodes[ds] = GetSubstringBeforeDashOrSlash(barCodes[ds]);
                        //var oi = await GetOrderItem(products[ds], int.Parse(quantities[ds]));
                        //if (oi != null)
                        //    order1.OrderItems.Add(oi);
                        //else
                        //{
                        //    //TODO
                        //}
                    }
                //}
                using (FileStream outputFileStream = new FileStream($"{fileName}-modified.pdf", FileMode.Create))
                {
                    loadedDocument.Save(outputFileStream);
                }
                string data = "";

                data = "-Invoice Number: " + invoiceNumer;
                data = "Invoice Number: " + invoiceNumer;
                data += "\n\nCostumer ID: " + costumerId;
                data += "\n\nBill To: " + billTo;
                data += "\n\nShip To: " + shipTo;
                data += "\n\nShip Date: " + shipDate;
                data += "\n\nOrder: " + order;
                data += "\n\nTotal Net Price: " + totalNetPrice;
                data += "\n\n------------\nQTY:";
                foreach (var q in quantities)
                    data += "\n\n" + q;
                data += "\n\n------------\nProducts:";
                foreach (var p in products)
                    data += "\n\n" + p;
                data += "\n\n------------\nPrices:";
                foreach (var p in prices)
                    data += "\n\n" + p;
                data += "\n\n------------\n VAT Prices:";
                foreach (var p in vatRate)
                    data += "\n\n" + p;

                data += "\n\n------------\n description:";
                foreach (var p in description)
                    data += "\n\n" + p;
                Console.WriteLine(data);

            }

            //foreach (OrderItem item in order1.OrderItems)
            //{
            //    Product? pord = await xCODESImports.Import("me", shopid, item.Name, null, null);
            //    if (pord != null)
            //    {
            //        item.Product = pord;
            //    }

            //}

            //await CreatStockIn(fileName, staffid);
        }

        //private async Task<OrderItem> GetOrderItem(string barcode, int qty)
        //{
        //    try
        //    {
        //        var orderItem = await GetOrderItemByBarcode(barcode);
        //        if (orderItem is null)
        //        {

        //            var result = await xCODESImports.Import("", shopid, barcode, null);
        //            if (result == null)
        //                throw new Exception("Not Found");



        //            //Search for PriceBreaks by barcode after import the results from XCode
        //            orderItem = await GetOrderItemByBarcode(barcode);
        //            if (orderItem is null)
        //                throw new Exception("Price breaks missing");

        //        }

        //        if (orderItem != null)
        //        {
        //            orderItem.Quantity = qty;
        //            orderItem.OrderQuantity = qty;
        //            return orderItem;
        //        }
        //        return null;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //private async Task<OrderItem> GetOrderItemByBarcode(string? search)
        //{
        //    var catShops = await _mainDbContext.ProductCatalogueShops.Where(x => x.ShopID == shopid).Select(x => new { x.ProductCatalogue_ID, x.SalesPrice_ShopID, x.CostPrice_ShopID }).ToArrayAsync();

        //    foreach (var catShop in catShops)
        //    {
        //        var saleShopId = catShop.SalesPrice_ShopID ?? shopid;
        //        var costShopId = catShop.CostPrice_ShopID ?? shopid;
        //        int.TryParse(search, out int barcode);
        //        var priceBreakIds = await (
        //            from pb in _mainDbContext.ProductVariantPriceBreaks
        //            from b in _mainDbContext.ProductVariantPriceBreakBarcodes.Where(x => x.ProductVariantPriceBreak_ID == pb.ID)
        //            where pb.ShopID == saleShopId && (b.Barcode == search || (barcode > 0 && pb.SalesLabelBarcode == barcode))
        //            select pb.ID).ToListAsync();

        //        if (priceBreakIds.Any())
        //        {
        //            var oi = await (from pb in _mainDbContext.ProductVariantPriceBreaks
        //                            from pv in _mainDbContext.ProductVariants.Where(x => x.ID == pb.ProductVariant_ID)
        //                            from pvc in _mainDbContext.ProductVariantCostPrices.Where(x => x.ProductVariant_ID == pv.ID && x.StockUnits == pb.StockUnits)
        //                            from p in _mainDbContext.Products.Where(x => x.ID == pv.Product_ID)
        //                            from pm in _mainDbContext.ProductMultisites.Where(x => x.Product_ID == p.ID)
        //                            where p.ProductCatalogue_ID == catShop.ProductCatalogue_ID && pb.ShopID == saleShopId
        //                            && pm.ShopID == shopid && pb.ID == priceBreakIds[0]
        //                            select new OrderItem
        //                            {
        //                                ProductVariantPriceBreak_ID = pb.ID,
        //                                CostPrice = pvc.CostPrice,
        //                                ProductVariant_ID = pv.ID,
        //                                //ImageURL = string.Empty,
        //                                SalesPrice = pb.SalesPrice,
        //                                StockUnit = pb.StockUnits,
        //                                VATCode = pb.VATCode,
        //                            }).FirstOrDefaultAsync();
        //            if (oi != null)
        //                return oi;
        //        }
        //    }

        //    return null;
        //}
    }
}
