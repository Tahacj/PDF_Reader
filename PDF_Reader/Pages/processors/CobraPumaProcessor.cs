using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XPOS.Shared.Models;

namespace PDF_Reader.Pages
{
    public class CobraPumaProcessor : ABrandTrackProcessor, IBrandTrackProcessor
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

        //private XCODESImports xCODESImports = new();
        //private MainDbContext _mainDbContext = new();
        private string shopid;

        //public CobraPumaProcessor(string shopid)
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

        public async Task ExtractData(Stream file , string fileName)
        {
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(file);


            // Loop in the pages of the loaded PDF document
            for (int i = 0; i < (loadedDocument.Pages.Count); i++)
            {
                PdfPageBase page = loadedDocument.Pages[i];
                //Console.WriteLine("width " + page.Size.Width + "\n hight " + page.Size.Height);
                // Extract text from the first page with bounds
                page.ExtractText(out TextLineCollection lineCollection);
                PdfGraphics graphics = page.Graphics;

                bool qty = false, prod = false, price = false;
                foreach (var txtLine in lineCollection.TextLine)
                {
                    //DrawRectangle(graphics, txtLine.Bounds, Color.Orange);
                    foreach (TextWord word in txtLine.WordCollection)
                    {
                        if (IsIntersected(qtyBounds, word.Bounds) || qty)
                            if ((word.Text.Length > 0 && float.TryParse(word.Text, out float qnty)) || qty)
                            {
                                if (word.Bounds.Y - (productsBounds.Y + productsBounds.Height) > 10)
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
                                if (word.Bounds.Y - (productsBounds.Y + productsBounds.Height) > 10)
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
                                if (word.Bounds.Y - (productsBounds.Y + productsBounds.Height) > 10)
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
                        discountAmountBounds.Height = qtyBounds.Height;
                        //totalNetPriceBounds = new RectangleF(526, productsBounds.Y + productsBounds.Height + 37, 30, 10);
                    }
                }

                DrawRectangle(graphics, invoiceBounds, Color.Red);
                DrawRectangle(graphics, costumerIdeBounds, Color.Blue);
                DrawRectangle(graphics, billToBounds, Color.Green);
                DrawRectangle(graphics, shipToBounds, Color.Gold);
                DrawRectangle(graphics, shipDateBounds, Color.GreenYellow);
                DrawRectangle(graphics, orderBounds, Color.HotPink);
                DrawRectangle(graphics, discountAmountBounds, Color.Black);
                DrawRectangle(graphics, totalNetPriceBounds, Color.Black);
                DrawRectangle(graphics, qtyBounds, Color.Orange);
                DrawRectangle(graphics, productsBounds, Color.Olive);
                DrawRectangle(graphics, priceBounds, Color.Purple);


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

                float total = 0;
                for (int j = 0; j < quantities.LongCount(); j++)
                {
                    if (!string.IsNullOrWhiteSpace(quantities[j]) && quantities[j] != "")
                    {
                        total += (
                              int.Parse(quantities[j].Trim())
                              * float.Parse(prices[j].Trim())
                              * (1 - (float.Parse(discountAmount[0].Trim()) / 100)));
                    }
                }

                //if (Pricecheck(total, totalNetPrice))
                //{
                //    order1.OrderType = OrderType.StockIn;
                //    order1.InvoiceNo = invoiceNumer;
                //    order1.Process = OrderProcess.Brandtrac;
                //    order1.Quantity = totalQuantity;
                //    order1.SupplierID = "Cobra - Puma Nordic AB";
                //    for (int ds = 0; ds < quantities.Count; ds++)
                //    {
                //        //barCodes[ds] = GetSubstringBeforeDashOrSlash(barCodes[ds]);
                //        //var oi = await GetOrderItem(products[ds], int.Parse(quantities[ds]));
                //        //if (oi != null)
                //        //    order1.OrderItems.Add(oi);
                //        //else
                //        //{
                //        //    //TODO
                //        //}
                //    }
                //}
                //else
                //{
                //    // skip file and make a function to retrun the fualted file
                //    break;
                //}
            }

            using (FileStream outputFileStream = new FileStream($"{fileName}-modified.pdf", FileMode.Create))
            {
                loadedDocument.Save(outputFileStream);
            }
            //loadedDocument.Close(true);
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
