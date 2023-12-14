using System.ComponentModel.DataAnnotations.Schema;

namespace PDF_Reader.Models
{
    public class Sale
    {
        public int ID { get; set; }
        public string? ShopID { get; set; }
        [ForeignKey("Customer")]
        public int Customer_ID { get; set; }
        //public Customer? Customer { get; set; }
        public int TransactionNumber { get; set; }
        public int? ReturnTransactionNumber { get; set; }
        public int Terminal_ID { get; set; }
        [ForeignKey(nameof(Terminal_ID))]
        //public Terminal? Terminal { get; set; }
        public int TerminalSession_ID { get; set; }
        [ForeignKey("Sale_ID")]
        public List<SaleDetail> SaleDetails { get; set; }
        //[ForeignKey("Sale_ID")]
        //public List<SalePayment> Payments { get; set; }
        public int Staff_ID { get; set; }
        [ForeignKey(nameof(Staff_ID))]
        //public Staff? Staff { get; set; }
        //public SaleStatus Status { get; set; }
        public decimal Cost { get; set; }
        public decimal? SalesPriceGross { get; set; }
        public decimal? SalesPriceNet { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Discount { get; set; }
        public string? Note { get; set; }
        public string? ReceiptNote { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public DateTime? TranscationDateTime { get; set; }
        public bool Export { get; set; }
        public bool? ForceReceipt { get; set; }

        //public XPOS.Shared.Enum.Process Process { get; set; }
    }
}
