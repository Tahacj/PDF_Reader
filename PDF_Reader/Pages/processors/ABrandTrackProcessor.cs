using Syncfusion.Drawing;
using Syncfusion.Pdf;
using System.Collections.Generic;
using System;
using XPOS.Shared.Models;
using Syncfusion.Pdf.Graphics;

namespace PDF_Reader.Pages
{
    public abstract class ABrandTrackProcessor
    {
        internal bool IsIntersected(RectangleF rect1, RectangleF rect2)
        {
            if (rect2.X < rect1.X + rect1.Width && rect1.X < rect2.X + rect2.Width && rect2.Y < rect1.Y + rect1.Height)
            {
                return rect1.Y < rect2.Y + rect2.Height;
            }

            return false;
        }

        public List<string> ExtractTextFromRectangle(TextLineCollection page, RectangleF qtyRec, RectangleF priceRec, RectangleF discountRec, RectangleF productRec, RectangleF descriptionRec, RectangleF VATRec)
        {
            string qty = "";
            string price = "";
            string discount = "";
            string product = "";
            string description = "";
            string VAT = "";
            foreach (var txtLine in page.TextLine)
            {
                foreach (TextWord word in txtLine.WordCollection)
                {
                    if (IsIntersected(qtyRec, word.Bounds))
                    {
                        qty += word.Text.Trim();
                    }
                    if (IsIntersected(priceRec, word.Bounds))
                    {
                        price += word.Text.Trim();
                    }
                    if (IsIntersected(discountRec, word.Bounds))
                    {
                        discount += word.Text.Trim();
                    }
                    if (IsIntersected(productRec, word.Bounds))
                    {
                        product += word.Text.Trim();
                    }
                    if (IsIntersected(descriptionRec, word.Bounds))
                    {
                        description += word.Text;
                    }
                    if (IsIntersected(VATRec, word.Bounds))
                    {
                        VAT += word.Text.Trim();
                    }
                }
            }

            return new List<string> { qty, price, discount, product, description, VAT };
        }

        public List<string> ExtractTextFromRectangleForNoDiscount(TextLineCollection page, RectangleF qtyRec, RectangleF priceRec, RectangleF productRec)
        {
            string qty = "";
            string price = "";
            string product = "";
            foreach (var txtLine in page.TextLine)
            {
                foreach (TextWord word in txtLine.WordCollection)
                {
                    if (IsIntersected(qtyRec, word.Bounds))
                    {
                        qty += word.Text;
                    }
                    if (IsIntersected(priceRec, word.Bounds))
                    {
                        price += word.Text;
                    }
                    if (IsIntersected(productRec, word.Bounds))
                    {
                        product += word.Text;
                    }
                }
            }

            return new List<string> { qty, price, product };
        }

        public bool Pricecheck(float calculated, string total)
        {
            if (Math.Abs(float.Parse(total.Trim()) - calculated) < 0.0001)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Drawing reqtangle on the selected area
        public void DrawRectangle(PdfGraphics graphics, RectangleF bounds, Color color)
        {
            PdfPen pen = new PdfPen(new PdfColor(color), 2f);
            graphics.DrawRectangle(pen, bounds);
        }

        //public async Task TransferBlobAsync(string blobName)
        //{
        //    //BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri("https://vcloudimages.blob.core.windows.net"), storageCredentials);
        //    //BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("invoices");


        //    StorageSharedKeyCredential storageCredentials = new StorageSharedKeyCredential("vcloudimages", "0wWeL2zbMrJZjGzWoZBAQ906LuN64v29J30WVprG7N/SdxygZmHo018g2cTbYHu7dKVwVPMWNcJqwTyNIjWlqA==");
        //    // Create a BlobServiceClient object which will be used to create a container client
        //    BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri("https://vcloudimages.blob.core.windows.net"), storageCredentials);

        //    // Create the blob clients for source and destination containers
        //    BlobContainerClient sourceContainerClient = blobServiceClient.GetBlobContainerClient("invoices");
        //    BlobContainerClient destContainerClient = blobServiceClient.GetBlobContainerClient("stockInInvoices");

        //    // Get a reference to a blob
        //    BlobClient sourceBlobClient = sourceContainerClient.GetBlobClient(blobName);

        //    // Check if the blob exists
        //    if (await sourceBlobClient.ExistsAsync())
        //    {
        //        // Get a reference to a blob
        //        BlobClient destBlobClient = destContainerClient.GetBlobClient(blobName);

        //        // Get the source blob's properties
        //        BlobProperties properties = await sourceBlobClient.GetPropertiesAsync();

        //        // Start the copy operation
        //        //await destBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);
        //    }
        //    else
        //    {
        //        Console.WriteLine($"The blob {blobName} does not exist in the source container.");
        //    }
        //}

        //public async Task DeleteBlobAsync(string blobName)
        //{
        //    StorageSharedKeyCredential storageCredentials = new StorageSharedKeyCredential("vcloudimages", "0wWeL2zbMrJZjGzWoZBAQ906LuN64v29J30WVprG7N/SdxygZmHo018g2cTbYHu7dKVwVPMWNcJqwTyNIjWlqA==");
        //    // Create a BlobServiceClient object which will be used to create a container client
        //    BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri("https://vcloudimages.blob.core.windows.net"), storageCredentials);

        //    // Create the blob client for the container
        //    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("invoices");

        //    // Get a reference to a blob
        //    BlobClient blobClient = containerClient.GetBlobClient(blobName);

        //    // Delete the blob
        //    await blobClient.DeleteIfExistsAsync();
        //}

        public static string GetSubstringBeforeDashOrSlash(string input)
        {
            string[] parts = { input };
            if (input.Contains("-"))
            {
                parts = input.Split('-');
            }
            if (input.Contains("/"))
            {
                parts = input.Split('/');
            }
            return parts[0];
        }

    }
}
