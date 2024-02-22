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
using PDF_Reader.Models;

namespace PDF_Reader.BrandtracProcessor
{
    public class CallawayProcessor : BaseProcessor, IBrandTrackProcessor
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
        static int height = 24;
        RectangleF invoiceBounds = new RectangleF(21, 219, 60, 10); //
        RectangleF costumerIdeBounds = new RectangleF(21, 244, 65, 10); //
        RectangleF billToBounds = new RectangleF(51, 125, 182, 80);
        RectangleF shipToBounds = new RectangleF(50, 30, 185, 83);
        RectangleF shipDateBounds = new RectangleF(171, 220, 50, 10);
        RectangleF orderBounds = new RectangleF(147, 244, 67, 10);
        RectangleF qtyBounds = new RectangleF(330, 300, 25, 20);
        RectangleF descriptionBounds = new RectangleF(129, 300, 174, 20);
        RectangleF productsBounds = new RectangleF(42, 300, 85, 20);
        RectangleF priceBounds = new RectangleF(390, 300, 40, 20);
        RectangleF VATBounds = new RectangleF(534, 300, 39, 20);
        RectangleF discountAmountBounds = new RectangleF(435, 300, 25, 20);
        RectangleF totalNetPriceBounds = new RectangleF(516, 648, 54, 12);

        List<RectangleF> qtyRectangles = new List<RectangleF>();
        List<RectangleF> priceRectangles = new List<RectangleF>();
        List<RectangleF> discountRectangles = new List<RectangleF>();
        List<RectangleF> productsRectangles = new List<RectangleF>();
        List<RectangleF> VATRectangles = new List<RectangleF>();
        List<RectangleF> DescriptionsRectangles = new List<RectangleF>();

        private string shopid;
        private string groupID;

        //public CallawayProcessor(string shopid)
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

            for (int i = 0; i < loadedDocument.Pages.Count; i++)
            {
                PdfPageBase page = loadedDocument.Pages[i];
                page.ExtractText(out TextLineCollection lineCollection);
                PdfGraphics graphics = page.Graphics;
                bool qty = false, prod = false, price = false, changed = false;
                foreach (var txtLine in lineCollection.TextLine)
                {
                    foreach (TextWord word in txtLine.WordCollection)
                    {
                        if (BrandTracUtils.IsIntersected(qtyBounds, word.Bounds))
                            if (word.Text.Length > 0 && float.TryParse(word.Text, out float qnty))
                                qty = true;
                        if (BrandTracUtils.IsIntersected(productsBounds, word.Bounds))
                            if (word.Text.Length > 0)
                                prod = true;
                        if (BrandTracUtils.IsIntersected(priceBounds, word.Bounds) && float.TryParse(word.Text, out float prc))
                            if (word.Text.Length > 0)
                                price = true;
                    }
                    if (qty && prod && price)
                    {


                        DrawRectangle(graphics, qtyBounds, Color.Orange);
                        DrawRectangle(graphics, productsBounds, Color.Olive);
                        DrawRectangle(graphics, descriptionBounds, Color.Black);
                        DrawRectangle(graphics, priceBounds, Color.Purple);
                        DrawRectangle(graphics, VATBounds, Color.Cyan);
                        DrawRectangle(graphics, discountAmountBounds, Color.Black);

                        qtyRectangles.Add(qtyBounds);
                        priceRectangles.Add(priceBounds);
                        discountRectangles.Add(discountAmountBounds);
                        productsRectangles.Add(productsBounds);
                        DescriptionsRectangles.Add(descriptionBounds);
                        VATRectangles.Add(VATBounds);
                        qtyBounds.Y += height;
                        priceBounds.Y += height;
                        discountAmountBounds.Y += height;
                        productsBounds.Y += height;
                        descriptionBounds.Y += height;
                        VATBounds.Y += height;

                        qty = false; prod = false; price = false; changed = true;
                    }
                }

                if (!changed)
                {
                    qtyBounds = new RectangleF(0, 0, 0, 0);
                    productsBounds = new RectangleF(0, 0, 0, 0);
                    priceBounds = new RectangleF(0, 0, 0, 0);
                    discountAmountBounds = new RectangleF(0, 0, 0, 0);
                    descriptionBounds = new RectangleF(0, 0, 0, 0);
                    VATBounds = new RectangleF(0, 0, 0, 0);
                }

                foreach (var txtLine in lineCollection.TextLine)
                {
                    foreach (TextWord word in txtLine.WordCollection)
                    {
                        if (i == 0)
                        {
                            if (BrandTracUtils.IsIntersected(invoiceBounds, word.Bounds))
                                invoiceNumer += word.Text.Trim();
                            if (BrandTracUtils.IsIntersected(costumerIdeBounds, word.Bounds))
                                costumerId += word.Text.Trim();
                            if (BrandTracUtils.IsIntersected(billToBounds, word.Bounds))
                                billTo += word.Text.Trim();
                            if (BrandTracUtils.IsIntersected(shipToBounds, word.Bounds))
                                shipTo += word.Text.Trim();
                            if (BrandTracUtils.IsIntersected(shipDateBounds, word.Bounds))
                                shipDate += word.Text.Trim();
                            if (BrandTracUtils.IsIntersected(orderBounds, word.Bounds))
                                order += word.Text.Trim();
                        }

                        if (BrandTracUtils.IsIntersected(totalNetPriceBounds, word.Bounds) && i == lastPageNum)
                            totalNetPrice += word.Text.Trim();
                    }
                }



                DrawRectangle(graphics, invoiceBounds, Color.Red);
                DrawRectangle(graphics, costumerIdeBounds, Color.Blue);
                DrawRectangle(graphics, billToBounds, Color.Green);
                DrawRectangle(graphics, shipToBounds, Color.Gold);
                DrawRectangle(graphics, shipDateBounds, Color.GreenYellow);
                DrawRectangle(graphics, orderBounds, Color.HotPink);
                DrawRectangle(graphics, totalNetPriceBounds, Color.Black);

                for (int r = 0; r < qtyRectangles.Count; r++)
                {
                    try
                    {
                        ExtractedProduct tempOrder = GetExtractProduct(lineCollection, qtyRectangles[r], priceRectangles[r], discountRectangles[r], productsRectangles[r], DescriptionsRectangles[r], VATRectangles[r]);
                        extractedOrders.Add(tempOrder);
                        totalQuantity += int.Parse(tempOrder.Qty);

                    }
                    catch (Exception e)
                    {
                        break;
                        //throw e;
                    }
                }



                float total = 0;
                for (int j = 0; j < extractedOrders.Count; j++)
                {
                    try
                    {
                        total += (
                            int.Parse(extractedOrders[j].Qty)
                           * float.Parse(extractedOrders[j].Price)
                           * (1f - (float.Parse(extractedOrders[j].Discount) / 100)));
                    }catch(Exception e)
                    {
                        break;
                    }
                }

                if (Pricecheck(total, totalNetPrice))
                {
                    //order1.CostPrice = (decimal)total;
                    //order1.OrderType = OrderType.StockIn;
                    //order1.InvoiceNo = invoiceNumer;
                    //order1.Process = OrderProcess.Brandtrac;
                    //order1.Quantity += totalQuantity;
                    //order1.SupplierID = "c_callaway";
                    //order1.ShopID = await FindShopId(costumerId);
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
                    break;
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