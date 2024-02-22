using PDF_Reader.Models;
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

namespace PDF_Reader.Pages
{
    public class ClevelandProcessor : BaseProcessor , IBrandTrackProcessor
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

        static int height = 35;
        RectangleF invoiceBounds = new RectangleF(486, 311, 52, 17);
        RectangleF costumerIdeBounds = new RectangleF(24, 354, 45, 10);
        RectangleF billToBounds = new RectangleF(35, 156, 195, 84);
        RectangleF shipToBounds = new RectangleF(368, 154, 195, 87);
        RectangleF shipDateBounds = new RectangleF(416, 311, 49, 16);
        RectangleF orderBounds = new RectangleF(400, 263, 33, 10);
        RectangleF qtyBounds = new RectangleF(248, 398, 42, height);
        RectangleF descriptionBounds = new RectangleF(91, 398, 137, height);
        RectangleF productsBounds = new RectangleF(28, 398, 64, height);
        RectangleF priceBounds = new RectangleF(297, 398, 33, height);
        RectangleF discountAmountBounds = new RectangleF(340, 398, 40, height);
        RectangleF totalNetPriceBounds = new RectangleF(524, 686, 50, 11);


        private string shopid;

        //public ClevelandProcessor(string shopid)
        //{
        //    this.shopid = shopid;
        //}

        
        public async Task<Order> ExtractData(Stream file, string fileName)
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
                        qtyBounds.Height += height;
                        productsBounds.Height += height;
                        priceBounds.Height += height;
                        discountAmountBounds.Height += height;

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
                else
                {
                    qtyBounds.Height -= height;
                    productsBounds.Height -= height;
                    priceBounds.Height -= height;
                    discountAmountBounds.Height -= height;
                }

                orderBounds = new RectangleF(32, productsBounds.Y + productsBounds.Height + 20, 50, 18);

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
                        if (IsIntersected(totalNetPriceBounds, word.Bounds) && i == lastPageNum)
                            totalNetPrice += word.Text;

                    }
                }

                float total = 0;
                for (int j = 0; j < quantities.LongCount(); j++)
                {
                    if (!string.IsNullOrWhiteSpace(quantities[j]) && quantities[j] != "")
                    {
                        total += (
                               int.Parse(quantities[i].Trim())
                               * float.Parse(prices[i].Trim())
                               * (1 - (float.Parse(discountAmount[i].Trim()) / 100)));
                    }
                }

                if (Pricecheck(total, totalNetPrice))
                {
                    order1.OrderType = OrderType.StockIn;
                    order1.InvoiceNo = invoiceNumer;
                    order1.Process = OrderProcess.Brandtrac;
                    order1.Quantity = totalQuantity;
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
                DrawRectangle(graphics, descriptionBounds, Color.Black);
                DrawRectangle(graphics, priceBounds, Color.Purple);
            }

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
            //foreach (var net in vat)
            //    data += "\n\n" + net;
            //data += "\n\n------------\n description:";
            //foreach (var net in description)
            //    data += "\n\n" + net;
            Console.WriteLine(data);

            //foreach (OrderItem item in order1.OrderItems)
            //{
            //    Product? pord = await xCODESImports.Import("me", shopid, item.Name, null, null);
            //    if (pord != null)
            //    {
            //        item.Product = pord;
            //    }

            //}

            //await CreatStockIn(fileName, staffid);
            return order1;
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
