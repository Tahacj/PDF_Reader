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
    public class GlenmuirProcessor : BaseProcessor, IBrandTrackProcessor
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
        static int height = 15;
        RectangleF costumerIdeBounds = new RectangleF(560, 155, 110, 15);
        RectangleF invoiceBounds = new RectangleF(560, 40, 110, 15);
        RectangleF billToBounds = new RectangleF(60, 140, 220, 100);
        RectangleF shipToBounds = new RectangleF(330, 140, 220, 100);
        RectangleF shipDateBounds = new RectangleF(700, 125, 85, 15);
        RectangleF orderBounds = new RectangleF(640, 180, 70, 17);
        RectangleF qtyBounds = new RectangleF(555, 265, 40, 300);
        RectangleF productsBounds = new RectangleF(30, 300, 50, height);
        RectangleF priceBounds = new RectangleF(300, 300, 40, height);
        RectangleF descriptionBounds = new RectangleF(80, 300, 160, height);
        RectangleF discountBounds = new RectangleF(425, 300, 20, height);
        RectangleF totalNetPriceBounds = new RectangleF(0, 0, 0, 0);
        RectangleF vatBounds = new RectangleF(0, 0, 0, 0);

        List<RectangleF> qtyRectangles = new List<RectangleF>();
        List<RectangleF> priceRectangles = new List<RectangleF>();
        List<RectangleF> discountRectangles = new List<RectangleF>();
        List<RectangleF> productsRectangles = new List<RectangleF>();
        List<RectangleF> descriptionRectangles = new List<RectangleF>();
        List<RectangleF> vatRectangles = new List<RectangleF>();

        private string shopid;

        //public GlenmuirProcessor(string shopid)
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
                List<RectangleF> discountRectangles = new List<RectangleF>();

                //extractedOrders = new List<ExtractedProduct>();
                totalQuantity = 0;

                PdfPageBase page = loadedDocument.Pages[i];
                page.ExtractText(out TextLineCollection lineCollection);
                PdfGraphics graphics = page.Graphics;
                bool hasProducts = false;
                foreach (var txtLine in lineCollection.TextLine)
                {
                    foreach (TextWord word in txtLine.WordCollection)
                    {
                        if (txtLine.Text.Contains("Item") && txtLine.Text.Contains("Qty") && txtLine.Text.Contains("Unit Price"))
                        {
                            hasProducts = true;

                            if (word.Text.Contains("Qty"))
                                qtyBounds.X = word.Bounds.X;

                            if (word.Text.Contains("Price"))
                                priceBounds.X = word.Bounds.X - 20;
                            
                            if (word.Text.Contains("Discount"))
                                discountBounds.X = word.Bounds.X + 5;
                        }

                        if (hasProducts)
                        {
                            
                            if (BrandTracUtils.IsIntersected(qtyBounds, word.Bounds))
                                if (word.Text.Trim().Length > 0 && float.TryParse(word.Text, out float qnty))
                                {
                                    qtyRectangles.Add(word.Bounds);
                                    qtyBounds.Height += height + 5;
                                }

                            if (txtLine.Text.Contains("Subtotal"))
                            {
                                totalNetPriceBounds = txtLine.Bounds;
                                totalNetPriceBounds.X += 50;
                                vatBounds = txtLine.Bounds;
                                vatBounds.Y += 25;
                                vatBounds.X += 13;
                                vatBounds.Width = 17;
                            }
                            if (BrandTracUtils.IsIntersected(totalNetPriceBounds, word.Bounds))
                                totalNetPrice += float.TryParse(word.Text.Trim(), out float v) ? v : 0;
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
                    discountBounds.Y = qtyRectangles[j].Y;

                    productsBounds.Height = newHeight;
                    priceBounds.Height = newHeight;
                    descriptionBounds.Height = newHeight;
                    discountBounds.Height = newHeight;

                    productsRectangles.Add(productsBounds);
                    priceRectangles.Add(priceBounds);
                    descriptionRectangles.Add(descriptionBounds);
                    discountRectangles.Add(discountBounds);

                    DrawRectangle(graphics, qtyRectangles[j], Color.Orange);
                    DrawRectangle(graphics, productsBounds, Color.Olive);
                    DrawRectangle(graphics, descriptionBounds, Color.Black);
                    DrawRectangle(graphics, priceBounds, Color.Purple);
                    DrawRectangle(graphics, discountBounds, Color.Cyan);

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
                        ExtractedProduct tempOrder = GetExtractProduct(lineCollection, qtyRectangles[r], priceRectangles[r], discountRectangles[r], productsRectangles[r], descriptionRectangles[r], vatBounds);
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
                               * float.Parse(extractedOrders[j].Price)
                               * (1f - (float.Parse(extractedOrders[j].Discount) / 100)));
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
