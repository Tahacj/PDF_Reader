﻿using PDF_Reader.Models;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;

namespace PDF_Reader.Pages
{
    public class TaylorMadeProcessor : BaseProcessor, IBrandTrackProcessor
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

        static int height = 50;
        RectangleF invoiceBounds = new RectangleF(260, 106, 60, 15);
        RectangleF costumerIdeBounds = new RectangleF(120, 140, 60, 10);
        RectangleF billToBounds = new RectangleF(36, 150, 190, 85);
        RectangleF shipToBounds = new RectangleF(305, 150, 215, 85);
        RectangleF shipDateBounds = new RectangleF(335, 275, 60, 23);
        RectangleF orderBounds = new RectangleF(36, 275, 50, 20);
        RectangleF qtyBounds = new RectangleF(188, 335, 25, 30);
        RectangleF productsBounds = new RectangleF(35, 335, 40, height);
        RectangleF priceBounds = new RectangleF(300, 335, 46, height);
        RectangleF discountAmountBounds = new RectangleF(345, 335, 40, height);
        RectangleF totalNetPriceBounds = new RectangleF(290, 637, 69, 20);
        RectangleF descriptionBounds = new RectangleF(78, 335, 104, height);
        RectangleF vatBounds = new RectangleF(137, 636, 33, 10);

        List<RectangleF> qtyRectangles = new List<RectangleF>();
        List<RectangleF> priceRectangles = new List<RectangleF>();
        List<RectangleF> discountRectangles = new List<RectangleF>();
        List<RectangleF> productsRectangles = new List<RectangleF>();
        List<RectangleF> descriptionRectangles = new List<RectangleF>();


        private string shopid;
        private string groupID;


        //public TaylorMadeProcessor(string shopid)
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
                PdfPageBase page = loadedDocument.Pages[i];
                page.ExtractText(out TextLineCollection lineCollection);
                PdfGraphics graphics = page.Graphics;

                productsRectangles = new();
                priceRectangles = new();
                discountRectangles = new();
                descriptionRectangles = new();
                qtyRectangles = new();
                qtyBounds.Height = height;
                foreach (var txtLine in lineCollection.TextLine)
                {
                    foreach (TextWord word in txtLine.WordCollection)
                    {
                        if (txtLine.Text.Contains("Net Amount"))
                        {
                            totalNetPriceBounds.Y = txtLine.Bounds.Y + txtLine.Bounds.Height + 7;
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
                        if (i == loadedDocument.Pages.Count - 1)
                            if (BrandTracUtils.IsIntersected(totalNetPriceBounds, word.Bounds))
                                totalNetPrice += float.TryParse(word.Text.Trim(), out float v) ? v : 0;

                        if (BrandTracUtils.IsIntersected(qtyBounds, word.Bounds))
                            if (word.Text.Trim().Length > 0 && float.TryParse(word.Text, out float qnty))
                            {
                                qtyRectangles.Add(word.Bounds);
                                qtyBounds.Height += 30;
                            }else
                                qtyBounds.Height -= 30;
                    }
                }

                float newHeight = 0;
                for (int j = 0; j < qtyRectangles.Count; j++)
                {
                    if (qtyRectangles.Count - 1 > j)
                    {
                        newHeight = qtyRectangles[j + 1].Y - qtyRectangles[j].Y - 3;
                    }
                    else if(newHeight == 0) newHeight = height;

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


                    DrawRectangle(graphics, qtyBounds, Color.Orange);
                    DrawRectangle(graphics, productsBounds, Color.Olive);
                    DrawRectangle(graphics, descriptionBounds, Color.Black);
                    DrawRectangle(graphics, priceBounds, Color.Purple);
                    DrawRectangle(graphics, discountAmountBounds, Color.Black);

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
                    DrawRectangle(graphics, totalNetPriceBounds, Color.Black);
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
                        //    order1.CostPrice = (decimal)total;
                        //    order1.OrderType = OrderType.StockIn;
                        //    order1.InvoiceNo = invoiceNumer;
                        //    order1.Process = OrderProcess.Brandtrac;
                        //    order1.Quantity += totalQuantity;
                        //    order1.ShopID = await FindShopId(costumerId);
                        //    order1.SupplierID = "TaylorMade";
                        //    groupID = await GetNonClassifiedGroupId(shopid);
                        //    if (groupID == null)
                        //        throw new Exception("groupID not found or could not be created!");
                        //    for (int ds = 0; ds < extractedOrders.Count; ds++)
                        //    {
                        //        var oi = await GetOrderItemByExtractedProduct(extractedOrders[ds], order1.SupplierID, shopid, groupID, false);
                        //        if (oi != null)
                        //            order1.OrderItems.Add(oi);
                        //    }
                    }
                    else
                    {
                        continue;
                        //throw new Exception($"validation failed for this Invoice Number: {invoiceNumer} the calculated total : {total} , the scanned total : {totalNetPrice}");
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
            data += "\n\n------------\nDiscount:";
            foreach (var discount in extractedOrders)
                data += "\n\n-" + discount.Discount;
            Console.WriteLine(data);

            //order1.InvoiceUrl = await TransferBlobAsync(fileName);
            return order1;
        }

    }
}