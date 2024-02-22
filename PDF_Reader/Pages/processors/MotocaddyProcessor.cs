using PDF_Reader.Models;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;

namespace PDF_Reader.Pages
{
    public class MotocaddyProcessor : BaseProcessor, IBrandTrackProcessor
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
        static int height = 10;
        RectangleF invoiceBounds = new RectangleF(430, 89, 52, 12);
        RectangleF costumerIdeBounds = new RectangleF(427, 110, 43, 13);
        RectangleF billToBounds = new RectangleF(46, 179, 126, 70);
        RectangleF shipToBounds = new RectangleF(50, 273, 125, 68);
        RectangleF shipDateBounds = new RectangleF(428, 183, 53, 10);
        RectangleF orderBounds = new RectangleF(495, 208, 56, 11);
        RectangleF qtyBounds = new RectangleF(63, 355, 36, height);
        RectangleF productsBounds = new RectangleF(476, 355, 52, height);
        RectangleF priceBounds = new RectangleF(297, 355, 32, height);
        RectangleF vatBounds = new RectangleF(372, 355, 25, height);
        RectangleF descriptionBounds = new RectangleF(138, 355, 100, height);
        RectangleF discountAmountBounds = new RectangleF(0, 0, 0, 0);
        RectangleF totalNetPriceBounds = new RectangleF();

        List<RectangleF> qtyRectangles = new List<RectangleF>();
        List<RectangleF> priceRectangles = new List<RectangleF>();
        List<RectangleF> discountRectangles = new List<RectangleF>();
        List<RectangleF> productsRectangles = new List<RectangleF>();
        List<RectangleF> descriptionRectangles = new List<RectangleF>();
        List<RectangleF> vatRectangles = new List<RectangleF>();

        private string shopid;
        private string groupID;

        //public MotocaddyProcessor(string shopid)
        //{
        //    this.shopid = shopid;
        //}

        public async Task<Order> ExtractData(Stream file, string fileName)
        {
            Order order1 = new();
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(file);
            if (loadedDocument.Pages.Count == 0)
                throw new Exception("file has no pages");
            for (int i = 0; i < (loadedDocument.Pages.Count); i++)
            {
                //extractedOrders = new List<ExtractedProduct>();

                PdfPageBase page = loadedDocument.Pages[i];

                page.ExtractText(out TextLineCollection lineCollection);
                PdfGraphics graphics = page.Graphics;
                productsRectangles = [];
                priceRectangles = [];
                discountRectangles = [];
                descriptionRectangles = [];
                vatRectangles = [];
                qtyRectangles = [];
                if (i == 1)
                {
                    qtyBounds.Y = 178;
                }
                qtyBounds.Height = height;
                bool qty = false;
                foreach (var txtLine in lineCollection.TextLine)
                {
                    foreach (TextWord word in txtLine.WordCollection)
                    {
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
                        {
                            if (word.Text == "Subtotal")
                            {
                                totalNetPriceBounds = new RectangleF(word.Bounds.X + word.Bounds.Width + 15, word.Bounds.Y, 40, 7);
                            }

                            if (BrandTracUtils.IsIntersected(totalNetPriceBounds, word.Bounds))
                                totalNetPrice += float.TryParse(word.Text.Trim(), out float v) ? v : 0;
                        }

                        if (BrandTracUtils.IsIntersected(qtyBounds, word.Bounds) || qty)
                            if ((word.Text.Length > 0 && float.TryParse(word.Text, out float qnty)) || qty)
                            {
                                if (qtyBounds.Y >= word.Bounds.Y)
                                {
                                    qty = false;
                                }
                                else
                                {
                                    qty = true;
                                    qtyBounds.Height = (word.Bounds.Y - qtyBounds.Y + word.Bounds.Height + 2);
                                    qtyRectangles.Add(word.Bounds);
                                }
                            }
                    }
                }

                DrawRectangle(graphics, invoiceBounds, Color.Red);
                DrawRectangle(graphics, costumerIdeBounds, Color.Blue);
                DrawRectangle(graphics, billToBounds, Color.Green);
                DrawRectangle(graphics, shipToBounds, Color.Gold);
                DrawRectangle(graphics, shipDateBounds, Color.GreenYellow);
                DrawRectangle(graphics, orderBounds, Color.HotPink);
                DrawRectangle(graphics, totalNetPriceBounds, Color.Black);

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
                    vatBounds.Y = qtyRectangles[j].Y;

                    productsBounds.Height = newHeight;
                    priceBounds.Height = newHeight;
                    discountAmountBounds.Height = newHeight;
                    descriptionBounds.Height = newHeight;
                    vatBounds.Height = newHeight;

                    DrawRectangle(graphics, qtyBounds, Color.Orange);
                    DrawRectangle(graphics, productsBounds, Color.Olive);
                    DrawRectangle(graphics, descriptionBounds, Color.Black);
                    DrawRectangle(graphics, priceBounds, Color.Purple);
                    DrawRectangle(graphics, vatBounds, Color.Cyan);
                    DrawRectangle(graphics, discountAmountBounds, Color.Black);

                    productsRectangles.Add(productsBounds);
                    priceRectangles.Add(priceBounds);
                    discountRectangles.Add(discountAmountBounds);
                    descriptionRectangles.Add(descriptionBounds);
                    vatRectangles.Add(vatBounds);

                }

                //foreach (var txtLine in lineCollection.TextLine)
                //{
                //    foreach (TextWord word in txtLine.WordCollection)
                //    {
                //        if (i == 0)
                //        {
                //            if (BrandTracUtils.IsIntersected(invoiceBounds, word.Bounds))
                //                invoiceNumer += word.Text;
                //            if (BrandTracUtils.IsIntersected(costumerIdeBounds, word.Bounds))
                //                costumerId += word.Text;
                //            if (BrandTracUtils.IsIntersected(billToBounds, word.Bounds))
                //                billTo += word.Text;
                //            if (BrandTracUtils.IsIntersected(shipToBounds, word.Bounds))
                //                shipTo += word.Text;
                //            if (BrandTracUtils.IsIntersected(shipDateBounds, word.Bounds))
                //                shipDate += word.Text;
                //            if (BrandTracUtils.IsIntersected(orderBounds, word.Bounds))
                //                order += word.Text;
                //        }

                //        if (i == loadedDocument.Pages.Count - 1)
                //        {
                //            if (word.Text == "Subtotal")
                //            {
                //                totalNetPriceBounds = new RectangleF(word.Bounds.X + word.Bounds.Width + 15, word.Bounds.Y, 35, 7);
                //            }

                //            if (BrandTracUtils.IsIntersected(totalNetPriceBounds, word.Bounds))
                //                totalNetPrice += float.TryParse(word.Text.Trim(), out float v) ? v : 0;
                //        }
                //    }
                //}

                for (int r = 0; r < qtyRectangles.Count; r++)
                {
                    try
                    {
                        ExtractedProduct tempOrder = GetExtractProduct(lineCollection, qtyRectangles[r], priceRectangles[r], discountRectangles[r], productsRectangles[r], descriptionRectangles[r], vatRectangles[r]);
                        extractedOrders.Add(tempOrder);
                        totalQuantity += int.Parse(tempOrder.Qty);
                    }
                    catch (Exception e)
                    {
                        break;
                        //throw e;
                    }
                }

                if(i == loadedDocument.Pages.Count - 1)
                {
                    float total = 0;
                    for (int j = 0; j < extractedOrders.Count(); j++)
                    {
                        total += (
                           (int)Math.Round(double.Parse(extractedOrders[j].Qty.Trim()))
                           * float.Parse(extractedOrders[j].Price.Trim()));
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
                        //    var oi = await GetOrderItemByExtractedProduct(extractedOrders[ds], order1.SupplierID, shopid, groupID, false);
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
            data += "\n\n------------\nDiscount:";
            foreach (var discount in extractedOrders)
                data += "\n\n-" + discount.Discount;
            Console.WriteLine(data);
            //order1.InvoiceUrl = await TransferBlobAsync(fileName);
            return order1;
        }
    }
}
