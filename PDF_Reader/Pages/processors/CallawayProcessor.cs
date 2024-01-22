using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XPOS.Shared.Models;

namespace PDF_Reader.Pages
{
    public class CallawayProcessor : ABrandTrackProcessor , IBrandTrackProcessor
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
        static int height = 25;
        RectangleF invoiceBounds = new RectangleF(21, 219, 60, 10); //
        RectangleF costumerIdeBounds = new RectangleF(21, 244, 65, 10); //
        RectangleF billToBounds = new RectangleF(51, 125, 182, 80);
        RectangleF shipToBounds = new RectangleF(50, 30, 185, 83);
        RectangleF shipDateBounds = new RectangleF(171, 220, 50, 10);
        RectangleF orderBounds = new RectangleF(147, 244, 67, 10);
        RectangleF qtyBounds = new RectangleF(330, 300, 25, height);
        RectangleF productsBounds = new RectangleF(42, 300, 85, height);
        RectangleF priceBounds = new RectangleF(390, 300, 40, height);
        RectangleF discountAmountBounds = new RectangleF(435, 300, 25, height);
        RectangleF totalNetPriceBounds = new RectangleF(516, 648, 54, 12);

        List<RectangleF> qtyRectangles = new List<RectangleF>();
        List<RectangleF> priceRectangles = new List<RectangleF>();
        List<RectangleF> discountRectangles = new List<RectangleF>();
        List<RectangleF> productsRectangles = new List<RectangleF>();

        //private XCodesContext xCodesContext = new();
        //private XCODESImports xCODESImports = new();
        //private MainDbContext _mainDbContext = new();
        private string shopid;
        //public CallawayProcessor(string shopid)
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

                bool qty = false, prod = false, price = false, changed = false;
                foreach (var txtLine in lineCollection.TextLine)
                {
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
                        qtyRectangles.Add(qtyBounds);
                        priceRectangles.Add(priceBounds);
                        discountRectangles.Add(discountAmountBounds);
                        productsRectangles.Add(productsBounds);
                        qtyBounds.Y += height;
                        priceBounds.Y += height;
                        discountAmountBounds.Y += height;
                        productsBounds.Y += height;

                        qty = false; prod = false; price = false; changed = true;
                    }

                }

                if (!changed)
                {
                    qtyBounds = new RectangleF(0, 0, 0, 0);
                    productsBounds = new RectangleF(0, 0, 0, 0);
                    priceBounds = new RectangleF(0, 0, 0, 0);
                    discountAmountBounds = new RectangleF(0, 0, 0, 0);
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

                        if (IsIntersected(totalNetPriceBounds, word.Bounds) && i == lastPageNum)
                            totalNetPrice += word.Text;
                    }
                }

                for (int r = 0; r < qtyRectangles.Count; r++)
                {
                    List<string> tempList = ExtractTextFromRectangle(lineCollection, qtyRectangles[r], priceRectangles[r], discountRectangles[r], productsRectangles[r]);
                    quantities.Add(tempList[0]);
                    prices.Add(tempList[1]);
                    discountAmount.Add(tempList[2]);
                    products.Add(tempList[3]);
                    totalQuantity += int.Parse(tempList[0]);
                }



                float total = 0;
                for (int j = 0; j < quantities.LongCount(); j++)
                {
                    if (!string.IsNullOrWhiteSpace(quantities[j]) && quantities[j] != "")
                    {
                        total += (
                           int.Parse(quantities[j].Trim())
                           * float.Parse(prices[j].Trim())
                           * (1 - (float.Parse(discountAmount[j].TrimEnd('%')) / 100)));
                    }
                }

                if (Pricecheck(total, totalNetPrice))
                {
                    order1.OrderType = OrderType.StockIn;
                    order1.InvoiceNo = invoiceNumer;
                    order1.Process = OrderProcess.Brandtrac;
                    order1.Quantity = totalQuantity;
                    order1.SupplierID = "c_callaway";
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
                }
                else
                {
                    // skip file and make a function to retrun the fualted file
                    break;
                }
            }


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

        //    }catch (Exception ex)
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
        //        //int.TryParse(search, out int barcode);
        //        //var priceBreakIds = await (
        //        //    from pb in _mainDbContext.ProductVariantPriceBreaks
        //        //    from b in _mainDbContext.ProductVariantPriceBreakBarcodes.Where(x => x.ProductVariantPriceBreak_ID == pb.ID)
        //        //    where pb.ShopID == saleShopId && (b.Barcode == search || (barcode > 0 && pb.SalesLabelBarcode == barcode))
        //        //    select pb.ID).ToListAsync();

        //        //if (priceBreakIds.Any())
        //        //{
        //        var oi = await (from pb in _mainDbContext.ProductVariantPriceBreaks
        //                        from pv in _mainDbContext.ProductVariants.Where(x => x.ID == pb.ProductVariant_ID)
        //                        from pvc in _mainDbContext.ProductVariantCostPrices.Where(x => x.ProductVariant_ID == pv.ID && x.StockUnits == pb.StockUnits)
        //                        from p in _mainDbContext.Products.Where(x => x.ID == pv.Product_ID)
        //                        from pm in _mainDbContext.ProductMultisites.Where(x => x.Product_ID == p.ID)
        //                        where p.ProductCatalogue_ID == catShop.ProductCatalogue_ID && pb.ShopID == saleShopId
        //                        && pm.ShopID == shopid && pv.SKU == search
        //                        select new OrderItem
        //                        {
        //                            ProductVariantPriceBreak_ID = pb.ID,
        //                            CostPrice = pvc.CostPrice,
        //                            ProductVariant_ID = pv.ID,
        //                            //ImageURL = string.Empty,
        //                            SalesPrice = pb.SalesPrice,
        //                            StockUnit = pb.StockUnits,
        //                            VATCode = pb.VATCode,
        //                        }).FirstOrDefaultAsync();
        //        if (oi != null)
        //            return oi;
        //    }
        //    //}

        //    return null;
        //}

        //public async Task<bool?> IsValid(string productid)
        //{

        //    var barcodeProductId = xCodesContext.Barcodes.FirstOrDefault(x => x.Barcode == productid)?.ProductID ??
        //        xCodesContext.PriceBreaks.FirstOrDefault(x => x.Barcode == productid)?.ProductID;
        //    if (string.IsNullOrEmpty(barcodeProductId))
        //        return null;

        //    var productChild = await xCodesContext.Products.FirstOrDefaultAsync(p => p.ProductID == barcodeProductId && p.CurrentProduct == 1);
        //    if (productChild != null)
        //    {
        //        var productInXcodes = await xCodesContext.Products.FirstOrDefaultAsync(p => p.ProductID == productChild.StyleNumber && p.CurrentProduct == 1 && p.MatrixType == 2);
        //        if (productInXcodes == null)
        //            return null;
        //    }

        //    return true;

        //}

    }
}