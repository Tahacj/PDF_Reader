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
    public class AdidasProcessor : BaseProcessor, IBrandTrackProcessor
    {
        Order order1 = new();
        private List<ExtractedProduct> extractedOrders = new List<ExtractedProduct>();
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
        private string totalNetPrice = "";
        private string vatRate = "";
        static int height = 18;
        RectangleF costumerIdeBounds = new RectangleF(485, 87, 55, 10);
        RectangleF invoiceBounds = new RectangleF(485, 100, 55, 10);
        RectangleF billToBounds = new RectangleF(195, 85, 160, 75);
        RectangleF shipToBounds = new RectangleF(0, 85, 160, 70);
        RectangleF shipDateBounds = new RectangleF(50, 160, 50, 10);
        RectangleF orderBounds = new RectangleF(485, 125, 55, 10);
        RectangleF qtyBounds = new RectangleF(335, 240, 35, height);
        RectangleF productsBounds = new RectangleF(30, 240, 40, height);
        RectangleF priceBounds = new RectangleF(600, 240, 30, height);
        RectangleF descriptionBounds = new RectangleF(135, 240, 200, height);
        RectangleF totalNetPriceBounds = new RectangleF(750, 0, 50, 15);
        RectangleF vatBounds = new RectangleF(620, 0, 35, height);

        List<RectangleF> qtyRectangles = new List<RectangleF>();
        List<RectangleF> priceRectangles = new List<RectangleF>();
        List<RectangleF> discountRectangles = new List<RectangleF>();
        List<RectangleF> productsRectangles = new List<RectangleF>();
        List<RectangleF> descriptionRectangles = new List<RectangleF>();
        List<RectangleF> vatRectangles = new List<RectangleF>();

        private string shopid;

        //public AdidasProcessor(string shopid)
        //{
        //    this.shopid = shopid;
        //}



        public async Task<Order> ExtractData(Stream file, string fileName)
        {
            Order order1 = new();
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(file);
            int lastPageNum = loadedDocument.Pages.Count - 1;
            if (loadedDocument.Pages.Count == 0)
                throw new Exception("file has no pages");
            for (int i = 0; i < (loadedDocument.Pages.Count); i++)
            {

                List<RectangleF> qtyRectangles = new List<RectangleF>();
                List<RectangleF> priceRectangles = new List<RectangleF>();
                List<RectangleF> productsRectangles = new List<RectangleF>();
                List<RectangleF> descriptionRectangles = new List<RectangleF>();

                //extractedOrders = new List<ExtractedProduct>();
                totalQuantity = 0;

                PdfPageBase page = loadedDocument.Pages[i];
                page.ExtractText(out TextLineCollection lineCollection);
                PdfGraphics graphics = page.Graphics;

                foreach (var txtLine in lineCollection.TextLine)
                {
                    foreach (TextWord word in txtLine.WordCollection)
                    {
                        if (BrandTracUtils.IsIntersected(qtyBounds, word.Bounds))
                            if (word.Text.Trim().Length > 0 && float.TryParse(word.Text, out float qnty))
                            {
                                qtyRectangles.Add(word.Bounds);
                                qtyBounds.Height += height;
                            }

                        if (txtLine.Text.Contains("Net Total"))
                        {
                            totalNetPriceBounds.Y = txtLine.Bounds.Y;
                            vatBounds.Y = txtLine.Bounds.Y + 60;
                        }

                        if (i == 0)
                        {
                            if (BrandTracUtils.IsIntersected(invoiceBounds, word.Bounds))
                                invoiceNumer += word.Text;
                            if (BrandTracUtils.IsIntersected(costumerIdeBounds, word.Bounds))
                                costumerId += word.Text;
                            if (BrandTracUtils.IsIntersected(billToBounds, word.Bounds))
                                billTo += word.Text;
                            if (BrandTracUtils.IsIntersected(shipToBounds, word.Bounds))
                                shipTo += word.Text;
                            if (BrandTracUtils.IsIntersected(shipDateBounds, word.Bounds))
                                shipDate += word.Text;
                            if (BrandTracUtils.IsIntersected(orderBounds, word.Bounds))
                                order += word.Text;
                        }
                        if (BrandTracUtils.IsIntersected(totalNetPriceBounds, word.Bounds))
                            totalNetPrice += float.TryParse(word.Text.Trim(), out float v) ? v : 0;
                    }
                }

                for (int j = 0; j < qtyRectangles.Count; j++)
                {

                    float newHeight = 0;
                    if (qtyRectangles.Count - 1 > j)
                    {
                        newHeight = qtyRectangles[j + 1].Y - qtyRectangles[j].Y - 6;
                    }
                    else newHeight = height;

                    productsBounds.Y = qtyRectangles[j].Y;
                    priceBounds.Y = qtyRectangles[j].Y;
                    descriptionBounds.Y = qtyRectangles[j].Y;

                    //productsBounds.Height = newHeight;
                    priceBounds.Height = newHeight;
                    descriptionBounds.Height = newHeight;

                    productsRectangles.Add(productsBounds);
                    priceRectangles.Add(priceBounds);
                    descriptionRectangles.Add(descriptionBounds);

                    DrawRectangle(graphics, qtyRectangles[j], Color.Orange);
                    DrawRectangle(graphics, productsBounds, Color.Olive);
                    DrawRectangle(graphics, descriptionBounds, Color.Black);
                    DrawRectangle(graphics, priceBounds, Color.Purple);

                }


                DrawRectangle(graphics, invoiceBounds, Color.Red);
                DrawRectangle(graphics, costumerIdeBounds, Color.Blue);
                DrawRectangle(graphics, billToBounds, Color.Green);
                DrawRectangle(graphics, shipToBounds, Color.Gold);
                DrawRectangle(graphics, shipDateBounds, Color.GreenYellow);
                DrawRectangle(graphics, orderBounds, Color.HotPink);
                DrawRectangle(graphics, totalNetPriceBounds, Color.Black);
                DrawRectangle(graphics, vatBounds, Color.Black);

                for (int r = 0; r < qtyRectangles.Count; r++)
                {
                    try
                    {
                        ExtractedProduct tempOrder = GetExtractProduct(lineCollection, qtyRectangles[r], priceRectangles[r], null, productsRectangles[r], descriptionRectangles[r], vatBounds);
                        extractedOrders.Add(tempOrder);
                        totalQuantity += int.Parse(tempOrder.Qty);
                    }
                    catch (Exception e)
                    {
                        //throw e;
                        break;
                    }
                }

                if (i == loadedDocument.Pages.Count - 1)
                {
                    float total = 0;
                    try
                    {
                        for (int j = 0; j < extractedOrders.LongCount(); j++)
                        {
                            total += (
                                int.Parse(extractedOrders[j].Qty)
                               * float.Parse(extractedOrders[j].Price));
                        }
                    }
                    catch (Exception e)
                    {
                        //throw e;
                        continue;
                    }

                    if (Pricecheck(total, totalNetPrice))
                    {
                        //order1.CostPrice = (decimal)total;
                        //order1.OrderType = OrderType.StockIn;
                        //order1.InvoiceNo = invoiceNumer;
                        //order1.Process = OrderProcess.Brandtrac;
                        //order1.ShopID = await FindShopId(costumerId);
                        //order1.Quantity += totalQuantity;
                        //groupID = await GetNonClassifiedGroupId(shopid);
                        //if (groupID == null)
                        //    throw new Exception("groupID not found or could not be created!");
                        //for (int ds = 0; ds < extractedOrders.Count; ds++)
                        //{
                        //    var oi = await GetOrderItemByExtractedProduct(extractedOrders[ds], order1.SupplierID, shopid, groupID, true);
                        //    if (oi != null)
                        //        order1.OrderItems.Add(oi);
                        //}
                    }
                    else
                    {
                        //throw new Exception($"validation failed for this Invoice Number: {invoiceNumer} the calculated total : {total} , the scanned total : {totalNetPrice}");
                        continue;
                    }
                }
            }
            using (FileStream outputFileStream = new FileStream($"{fileName}-modified.pdf", FileMode.Create))
            {
                loadedDocument.Save(outputFileStream);
            }
            //order1.InvoiceUrl = await TransferBlobAsync(fileName);
            return order1;
        }
    }
}
