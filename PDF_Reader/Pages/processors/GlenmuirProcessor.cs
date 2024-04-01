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
        private string mainProductId = "";
        private string mainProductDescription = "";
        private int totalQuantity = 0;
        private string quantity;
        private string productId;
        private string prices;
        private string description;
        private string totalNetPrice = "";
        private string vatRate = "";
        private float total = 0;
        static int height = 13;
        RectangleF costumerIdeBounds = new RectangleF(560, 153, 110, 15);
        RectangleF invoiceBounds = new RectangleF(560, 40, 110, 15);
        RectangleF billToBounds = new RectangleF(60, 140, 220, 100);
        RectangleF shipToBounds = new RectangleF(330, 140, 220, 100);
        RectangleF shipDateBounds = new RectangleF(700, 125, 85, 15);
        RectangleF orderBounds = new RectangleF(640, 180, 70, 17);
        RectangleF qtyBounds = new RectangleF(0, 0, 0, 0);
        RectangleF productBounds = new RectangleF(25, 0, 50, height);
        RectangleF priceBounds = new RectangleF(600, 0, 40, height);
        RectangleF descriptionBounds = new RectangleF(0, 0, 0, height);
        RectangleF totalNetPriceBounds = new RectangleF(0, 0, 0, 0);
        RectangleF VATBounds = new RectangleF(0, 0, 0, 0);

        RectangleF mainProductBounds = new RectangleF(0, 0, 0, 0);
        RectangleF mainDescriptiontBounds = new RectangleF(0, 0, 0, 0);

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

                totalQuantity = 0;

                PdfPageBase page = loadedDocument.Pages[i];
                page.ExtractText(out TextLineCollection lineCollection);
                PdfGraphics graphics = page.Graphics;
                bool qty = false, prod = false, price = false;
                foreach (var txtLine in lineCollection.TextLine)
                {
                    foreach (TextWord word in txtLine.WordCollection)
                    {

                        if (IsIntersected(qtyBounds, word.Bounds))
                            if (word.Text.Length > 0 && float.TryParse(word.Text, out float qnty))
                            {
                                DrawRectangle(graphics, qtyBounds, Color.Purple);
                                quantity = word.Text;
                                qty = true;
                            }
                        if (IsIntersected(priceBounds, word.Bounds) && float.TryParse(word.Text, out float prc))
                            if (word.Text.Length > 0)
                            {
                                DrawRectangle(graphics, priceBounds, Color.Orange);
                                prices = word.Text;
                                qtyBounds.Y += height;
                                priceBounds.Y += height;
                                productBounds.Y += height;
                                descriptionBounds.Y += height;
                                price = true;
                            }
                        if (IsIntersected(productBounds, word.Bounds))
                            if (word.Text.Length > 0)
                            {
                                if (word.Text.Contains("Commodity"))
                                {
                                    mainProductBounds.Y = word.Bounds.Y + 27;
                                    productBounds.Y += 51;
                                    qtyBounds.Y += 51;
                                    priceBounds.Y += 51;
                                    descriptionBounds.Y += 51;
                                    break;
                                }
                                DrawRectangle(graphics, productBounds, Color.Cyan);
                                productId = word.Text;
                                descriptionBounds.X = productBounds.X + 50;
                                descriptionBounds.Width = 200 - descriptionBounds.X;
                                prod = true;
                            }
                        if (IsIntersected(descriptionBounds, word.Bounds))
                            if (word.Text.Length > 0)
                            {
                                DrawRectangle(graphics, descriptionBounds, Color.Olive);
                                description += word.Text;
                                price = true;
                            }

                        if (txtLine.Text.Contains("Style"))
                        {
                            if (word.Text.Contains("Style"))
                            {
                                mainProductBounds = word.Bounds;
                                mainProductBounds.Y += 18;
                            }

                            if (word.Text.Contains("Units"))
                            {
                                qtyBounds = word.Bounds;
                                qtyBounds.Y = word.Bounds.Y + 45;
                                priceBounds.Y = word.Bounds.Y + 45;
                                productBounds.Y = word.Bounds.Y + 45;
                                descriptionBounds = productBounds;
                                descriptionBounds.X = productBounds.X + 40;
                                descriptionBounds.Width = 200 - descriptionBounds.X;
                            }
                        }

                        if (IsIntersected(mainProductBounds, word.Bounds))
                        {
                            DrawRectangle(graphics, mainProductBounds, Color.DarkGoldenrod);
                            mainProductId = word.Text;
                            mainProductDescription = "";
                            mainDescriptiontBounds = word.Bounds;
                            mainDescriptiontBounds.X += word.Bounds.Width + 10;
                            mainDescriptiontBounds.Width = 400;
                        }

                        if (IsIntersected(mainDescriptiontBounds, word.Bounds))
                        {
                            mainProductDescription += word.Text;
                            DrawRectangle(graphics, mainDescriptiontBounds, Color.Green);
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
                        if (i == lastPageNum)
                        {
                            if (word.Text.Contains("Rate"))
                            {
                                VATBounds = word.Bounds;
                                VATBounds.Y += 10;
                            }
                            if (word.Text == "Net")
                            {
                                totalNetPriceBounds = word.Bounds;
                                totalNetPriceBounds.X += 120;
                                totalNetPriceBounds.Width = 75;
                            }

                            if (BrandTracUtils.IsIntersected(totalNetPriceBounds, word.Bounds))
                                totalNetPrice += float.TryParse(word.Text.Trim(), out float v) ? v : 0;

                            if (BrandTracUtils.IsIntersected(VATBounds, word.Bounds))
                                vatRate = word.Text.Trim().TrimEnd('%');
                        }
                    }
                    if (qty && prod && price)
                    {
                        ExtractedProduct tempProd = new ExtractedProduct
                        {
                            Qty = quantity,
                            Price = prices,
                            Name = mainProductId + "-" + productId,
                            Description = mainProductDescription + description,
                            Vat = vatRate
                        };
                        
                        extractedOrders.Add(tempProd);

                        description = "";

                        qty = false; prod = false; price = false;
                    }
                }

                if (i == 0)
                {
                    DrawRectangle(graphics, invoiceBounds, Color.Red);
                    DrawRectangle(graphics, costumerIdeBounds, Color.Blue);
                    DrawRectangle(graphics, billToBounds, Color.Green);
                    DrawRectangle(graphics, shipToBounds, Color.Gold);
                    DrawRectangle(graphics, shipDateBounds, Color.GreenYellow);
                    DrawRectangle(graphics, orderBounds, Color.HotPink);
                }

                if (i == lastPageNum)
                {
                    DrawRectangle(graphics, totalNetPriceBounds, Color.Black);
                    DrawRectangle(graphics, VATBounds, Color.Cyan);

                }

                //for (int r = 0; r < qtyRectangles.Count; r++)
                //{
                //    try
                //    {
                //        ExtractedProduct tempOrder = GetExtractProduct(lineCollection, qtyRectangles[r], priceRectangles[r], null, productsRectangles[r], descriptionRectangles[r], VATBounds);
                //        extractedOrders.Add(tempOrder);
                //        totalQuantity += int.Parse(tempOrder.Qty);
                //    }
                //    catch (Exception e)
                //    {
                //        //throw e;
                //        break;
                //    }
                //}

                if (i == loadedDocument.Pages.Count - 1)
                {

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
            string data = "";

            data = "Invoice Number: " + invoiceNumer;
            data += "\n\nCostumer ID: " + costumerId;
            data += "\n\nBill To: " + billTo;
            data += "\n\nShip To: " + shipTo;
            data += "\n\nShip Date: " + shipDate;
            data += "\n\nOrder: " + order;
            data += "\n\nTotal Net Price: " + totalNetPrice;
            data += "\n\n------------\nQTY:";
            foreach (var qty in extractedOrders)
                data += "\n\n" + qty.Qty;
            data += "\n\n------------\nProducts:";
            foreach (var p in extractedOrders)
                data += "\n\n" + p.Name;
            data += "\n\n------------\nPrices:";
            foreach (var p in extractedOrders)
                data += "\n\n" + p.Price;
            data += "\n\n------------\nDescription:";
            foreach (var p in extractedOrders)
                data += "\n\n-" + p.Description;
            Console.WriteLine(data);
            //order1.InvoiceUrl = await TransferBlobAsync(fileName);
            return order1;
        }
    }
}
