using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPOS.Shared.Models
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        public string? ShopID { get; set; }
        public string? MultisiteShopId { get; set; }
        public OrderType OrderType { get; set; }//
        public OrderProcess? Process { get; set; }//
        public string? StaffID { get; set; }
        public string? SupplierID { get; set; }
        public string? Reference { get; set; }
        public string? InvoiceNo { get; set; }//
        public string? ReferenceNo { get; set; }
        public string? DeliveryNo { get; set; }
        public decimal CostPrice { get; set; }
        public int Quantity { get; set; }//
        [NotMapped]
        public int? OrderQuantity { get; set; }
        public int UnitsReceived { get; set; }
        public DateTime? OrderReceived { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? AttachmentURL { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public bool Complete { get; set; }
        public decimal? Discount { get; set; }
        public string? Notes { get; set; }
        [NotMapped]
        public string? StaffName { get; set; }
        [NotMapped]
        public string? SupplierName { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();//
    }

    public enum OrderType { StockIn, StockOut }
    public enum OrderProcess
    {
        StockScreen = 0,
        StockTake = 1,
        Xapp = 2,
        Migrations = 3,
        V1Migrations = 4,
        SaleRefund = 5,
        GroupArchive = 6,
        BrandArchive = 7,
        ProductArchive = 8,
        VariantArchive = 9,
        VoucherArchive = 10,
        DataFix = 11,
        Brandtrac = 12,
    }
}
