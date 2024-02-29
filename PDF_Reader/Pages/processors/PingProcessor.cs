using PDF_Reader.Models;
using PDF_Reader.Pages;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PDF_Reader.Pages
{
    public class PingProcessor : BaseProcessor, IBrandTrackProcessor
    {
        private string invoiceNumer = "";
        private string costumerId = "";
        private string billTo = "";
        private string shipTo = "";
        private string shipDate = "";
        private string order = "";
        private int totalQuantity = 0;
        private List<ExtractedProduct> extractedOrders = new List<ExtractedProduct>();
        private string totalNetPrice = "";
        private string vatRate = "";
        static int height = 12;
        RectangleF invoiceBounds = new RectangleF(470, 33, 55, 15);
        RectangleF costumerIdeBounds = new RectangleF(29, 247, 45, 10);
        RectangleF billToBounds = new RectangleF(62, 150, 185, 75);
        RectangleF shipToBounds = new RectangleF(360, 150, 185, 75);
        RectangleF shipDateBounds = new RectangleF(90, 0, 60, 10);
        RectangleF orderBounds = new RectangleF(181, 247, 50, 10);
        RectangleF qtyBounds = new RectangleF(65, 290, 33, height);
        RectangleF productsBounds = new RectangleF(28, 290, 35, height);
        RectangleF priceBounds = new RectangleF(460, 290, 50, height);
        //RectangleF discountAmountBounds = new RectangleF(383, 372, 35, height);
        RectangleF descriptionBounds = new RectangleF(135, 290, 320, height);
        RectangleF VATBounds = new RectangleF(0, 0, 16, 10);
        RectangleF totalNetPriceBounds = new RectangleF(0, 0, 0, 0);

        List<RectangleF> qtyRectangles = new List<RectangleF>();
        List<RectangleF> priceRectangles = new List<RectangleF>();
        //List<RectangleF> discountRectangles = new List<RectangleF>();
        List<RectangleF> productsRectangles = new List<RectangleF>();
        List<RectangleF> descriptionRectangles = new List<RectangleF>();
        //List<RectangleF> VATRectangles = new List<RectangleF>();

        private string shopid;
        private string groupID;


        float total = 0;



        //public PingProcessor(string shopid)
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
                //List<RectangleF> discountRectangles = new List<RectangleF>();
                List<RectangleF> productsRectangles = new List<RectangleF>();
                List<RectangleF> descriptionRectangles = new List<RectangleF>();
                //List<RectangleF> VATRectangles = new List<RectangleF>();

                //extractedOrders = new List<ExtractedProduct>();
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
                                qty = true;
                            }
                        if (IsIntersected(descriptionBounds, word.Bounds))
                            if (word.Text.Length > 0)
                            {
                                prod = true;
                            }
                        if (IsIntersected(priceBounds, word.Bounds) && float.TryParse(word.Text, out float prc))
                            if (word.Text.Length > 0)
                            {
                                price = true;
                            }

                        if (txtLine.Text.Contains("INVOICE SUBTOTAL"))
                        {
                            totalNetPriceBounds = txtLine.Bounds;
                            totalNetPriceBounds.X += 145;
                            VATBounds.Y = txtLine.Bounds.Y + 22;
                            VATBounds.X = txtLine.Bounds.X + 51;
                        }

                        if (txtLine.Text.Contains("Ship Date"))
                        {
                            if (word.Text.Contains("Ship"))
                            {
                                shipDateBounds = word.Bounds;
                                shipDateBounds.Y += 14;
                                shipDateBounds.Width = 40;
                            }
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
                    if (!qty && prod && !price)
                    {
                        //float maxHeight = Math.Max(qtyBounds.Height, Math.Max(descriptionBounds.Height, priceBounds.Height));
                        //descriptionRectangles.Add(qtyBounds);

                        //qtyBounds.Height = maxHeight;
                        //productsBounds.Height = maxHeight;
                        //priceBounds.Height = maxHeight;

                        descriptionBounds.Height += height;
                        var temp = descriptionRectangles[descriptionRectangles.Count - 1];
                        temp.Height = descriptionBounds.Height;
                        descriptionRectangles[descriptionRectangles.Count - 1] = temp;

                        qtyBounds.Y += height;
                        priceBounds.Y += height;

                        qty = false; prod = false; price = false;
                    }
                    else if(qty && prod && price)
                    {

                        qtyRectangles.Add(qtyBounds);
                        descriptionRectangles.Add(descriptionBounds);
                        //priceRectangles.Add(priceBounds);
                        //DrawRectangle(graphics, descriptionBounds, Color.Cyan);

                        float newY = descriptionBounds.Height + descriptionBounds.Y;
                        //descriptionBounds.Height = height;
                        descriptionBounds.Y = newY;
                        qtyBounds.Y = newY;
                        priceBounds.Y = newY;


                        qty = false; prod = false; price = false;
                        //DrawRectangle(graphics, descriptionBounds, Color.Olive);
                        //DrawRectangle(graphics, qtyBounds, Color.Purple);
                        //DrawRectangle(graphics, priceBounds, Color.Orange);
                    }
                }

                //if (qtyRectangles.Count > 0)
                //{
                //    if (qtyRectangles[qtyRectangles.Count - 1].X - qtyRectangles[qtyRectangles.Count - 2].X < -10)
                //    {
                //        qtyRectangles.RemoveAt(qtyRectangles.Count - 1);
                //    }
                //}

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
                    //descriptionBounds.Y = qtyRectangles[j].Y;
                    //discountAmountBounds.Y = qtyRectangles[j].Y;
                    //VATBounds.Y = qtyRectangles[j].Y;

                    productsBounds.Height = newHeight;
                    priceBounds.Height = newHeight;
                    //descriptionBounds.Height = newHeight;
                    //discountAmountBounds.Height = newHeight;
                    //VATBounds.Height = newHeight;

                    productsRectangles.Add(productsBounds);
                    priceRectangles.Add(priceBounds);
                    //descriptionRectangles.Add(descriptionBounds);
                    //discountRectangles.Add(discountAmountBounds);
                    //VATRectangles.Add(VATBounds);

                    DrawRectangle(graphics, qtyRectangles[j], Color.Orange);
                    DrawRectangle(graphics, productsBounds, Color.Olive);
                    DrawRectangle(graphics, descriptionRectangles[j], Color.Black);
                    DrawRectangle(graphics, priceBounds, Color.Purple);
                    //DrawRectangle(graphics, discountAmountBounds, Color.Black);

                }


                DrawRectangle(graphics, invoiceBounds, Color.Red);
                DrawRectangle(graphics, costumerIdeBounds, Color.Blue);
                DrawRectangle(graphics, billToBounds, Color.Green);
                DrawRectangle(graphics, shipToBounds, Color.Gold);
                DrawRectangle(graphics, shipDateBounds, Color.GreenYellow);
                DrawRectangle(graphics, orderBounds, Color.HotPink);
                DrawRectangle(graphics, totalNetPriceBounds, Color.Black);
                DrawRectangle(graphics, VATBounds, Color.Cyan);

                for (int r = 0; r < qtyRectangles.Count; r++)
                {
                    try
                    {
                        ExtractedProduct tempOrder = GetExtractProduct(lineCollection, qtyRectangles[r], priceRectangles[r], null, productsRectangles[r], descriptionRectangles[r], VATBounds);
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
