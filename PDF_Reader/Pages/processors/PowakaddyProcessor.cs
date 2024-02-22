using PDF_Reader.Models;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;

namespace PDF_Reader.Pages
{
    public class PowakaddyProcessor : BaseProcessor, IBrandTrackProcessor
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
        static int height = 12;
        static float stratPosX = 0;
        static float stratPosY = 0;

        RectangleF invoiceBounds = new RectangleF(107 + stratPosX, 16 + stratPosY, 57, 10);
        RectangleF costumerIdeBounds = new RectangleF(107 + stratPosX, 53 + stratPosY, 53, 10);
        RectangleF billToBounds = new RectangleF(20 + stratPosX, 120 + stratPosY, 155, 65);
        RectangleF shipToBounds = new RectangleF(244 + stratPosX, 120 + stratPosY, 155, 65);
        RectangleF shipDateBounds = new RectangleF(111 + stratPosX, 30 + stratPosY, 46, 10);
        RectangleF orderBounds = new RectangleF(300 + stratPosX, 52 + stratPosY, 60, 12);
        RectangleF qtyBounds = new RectangleF(385 + stratPosX, 220 + stratPosY, 27, height);
        RectangleF descriptionBounds = new RectangleF(76 + stratPosX, 220 + stratPosY, 27, height);
        RectangleF productsBounds = new RectangleF(-9 + stratPosX, 220 + stratPosY, 77, height);
        RectangleF vatBounds = new RectangleF(107 + stratPosX, 584 + stratPosY, 25, 11);
        RectangleF priceBounds = new RectangleF(424 + stratPosX, 220 + stratPosY, 35, height);
        RectangleF discountAmountBounds = new RectangleF(460 + stratPosX, 220 + stratPosY, 20, height);
        RectangleF totalNetPriceBounds = new RectangleF(513 + stratPosX, 607 + stratPosY, 50, 15);


        List<RectangleF> qtyRectangles = new List<RectangleF>();
        List<RectangleF> priceRectangles = new List<RectangleF>();
        List<RectangleF> discountRectangles = new List<RectangleF>();
        List<RectangleF> productsRectangles = new List<RectangleF>();
        List<RectangleF> descriptionRectangles = new List<RectangleF>();
        List<RectangleF> vatRectangles = new List<RectangleF>();

        private string shopid;
        private string groupID;


        //public PowakaddyProcessor(string shopid)
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

                stratPosX = lineCollection.TextLine[9].Bounds.Location.X;
                stratPosY = lineCollection.TextLine[9].Bounds.Location.Y;

                invoiceBounds = new RectangleF(107 + stratPosX, 16 + stratPosY, 57, 10);
                costumerIdeBounds = new RectangleF(107 + stratPosX, 53 + stratPosY, 53, 10);
                billToBounds = new RectangleF(20 + stratPosX, 120 + stratPosY, 155, 65);
                shipToBounds = new RectangleF(244 + stratPosX, 120 + stratPosY, 155, 65);
                shipDateBounds = new RectangleF(111 + stratPosX, 30 + stratPosY, 46, 10);
                orderBounds = new RectangleF(300 + stratPosX, 52 + stratPosY, 60, 12);
                qtyBounds = new RectangleF(385 + stratPosX, 220 + stratPosY, 20, height);
                productsBounds = new RectangleF(-9 + stratPosX, 220 + stratPosY, 77, height);
                descriptionBounds = new RectangleF(76 + stratPosX, 220 + stratPosY, 200, height);
                priceBounds = new RectangleF(424 + stratPosX, 220 + stratPosY, 35, height);
                discountAmountBounds = new RectangleF(460 + stratPosX, 220 + stratPosY, 20, height);
                totalNetPriceBounds = new RectangleF(513 + stratPosX, 0 + stratPosY, 50, 15);
                vatBounds = new RectangleF(100 + stratPosX, 567 + stratPosY, 25, 11);

                foreach (var txtLine in lineCollection.TextLine)
                {
                    //DrawRectangle(graphics, txtLine.Bounds, Color.Orange);A
                    foreach (TextWord word in txtLine.WordCollection)
                    {
                        if (BrandTracUtils.IsIntersected(qtyBounds, word.Bounds))
                            if (word.Text.Trim().Length > 0 && float.TryParse(word.Text, out float qnty))
                            {
                                qtyRectangles.Add(word.Bounds);
                                qtyBounds.Y += height;
                            }

                        if (word.Text == "Rate")
                        {
                            vatBounds.Y = word.Bounds.Y + word.Bounds.Height + 2;
                            totalNetPriceBounds.Y = word.Bounds.Y + word.Bounds.Height - 5;
                        }

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
                    }
                }

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

                foreach (var txtLine in lineCollection.TextLine)
                {
                    foreach (TextWord word in txtLine.WordCollection)
                    {
                        if (BrandTracUtils.IsIntersected(totalNetPriceBounds, word.Bounds))
                            totalNetPrice += float.TryParse(word.Text.Trim(), out float v) ? v : 0;
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
                        ExtractedProduct tempOrder = GetExtractProduct(lineCollection, qtyRectangles[r], priceRectangles[r], discountRectangles[r], productsRectangles[r], descriptionRectangles[r], vatBounds);
                        extractedOrders.Add(tempOrder);
                        totalQuantity += (int)float.Parse(tempOrder.Qty);
                    }
                    catch (Exception e)
                    {
                        //throw e;
                        break;
                    }
                }


                float total = 0;
                try
                {
                    for (int j = 0; j < extractedOrders.LongCount(); j++)
                    {
                        total += (
                              (int)float.Parse(extractedOrders[j].Qty.Trim())
                              * float.Parse(extractedOrders[j].Price.Trim())
                              * (1f - (float.Parse(extractedOrders[j].Discount ?? "0") / 100)));
                    }
                }
                catch (Exception e)
                {
                    continue;
                    //throw e;
                }

                if (Pricecheck(total, totalNetPrice))
                {
                    //order1.CostPrice = (decimal)total;
                    //order1.OrderType = OrderType.StockIn;
                    //order1.InvoiceNo = invoiceNumer;
                    //order1.Process = OrderProcess.Brandtrac;
                    //order1.ShopID = await FindShopId(costumerId);
                    //order1.Quantity += totalQuantity;
                    //order1.SupplierID = "PowaKaddy International Limited";
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
                    continue;
                    //throw new Exception($"validation failed for this Invoice Number: {invoiceNumer} the calculated total : {total} , the scanned total : {totalNetPrice}");
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
