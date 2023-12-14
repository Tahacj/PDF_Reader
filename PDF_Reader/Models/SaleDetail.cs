using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PDF_Reader.Models
{
    public class SaleDetail
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey(nameof(Sale))]
        public int Sale_ID { get; set; }

        [JsonIgnore]
        public virtual Sale? Sale { get; set; }

        [ForeignKey("ProductVariant")]
        public int ProductVariant_ID { get; set; }
        //public virtual ProductVariant? ProductVariant { get; set; }
        public int? ProductVariantPriceBreak_ID { get; set; }
        [ForeignKey(nameof(ProductVariantPriceBreak_ID))]
        //public ProductVariantPriceBreak? ProductVariantPriceBreak { get; set; }
        public int? CustomerPriceBandID { get; set; }
        [ForeignKey(nameof(CustomerPriceBandID))]
        //public CustomerPriceBand? CustomerPriceBand { get; set; }
        public int? Company_ID { get; set; }
        [ForeignKey(nameof(Company_ID))]
        //public Company? Company { get; set; }
        public short Qty { get; set; }
        public decimal ShouldSalesPrice { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal ACP { get; set; }
        public string Note { get; set; }
        public string? Description { get; set; }
        public decimal? VAT { get; set; }
        public decimal? LineTotalVAT { get; set; }
        public decimal? VATRate { get; set; }
        [StringLength(20)]
        public string VATCode { get; set; }
        public decimal? Discount { get; set; }
        public decimal? ItemDiscount { get; set; }
        public decimal? BasketDiscount { get; set; }
        [NotMapped]
        public bool ZeroStocked { get; set; }

        public short? PriceBreakQty { get; set; }
        public decimal? PriceBreakSalePrice { get; set; }
        //public List<SaleDetailSerialNo>? SerialNos { get; set; }
        public int? CommissionStaff_ID { get; set; }
        public decimal? Commission { get; set; }
        public decimal? CommissionRate { get; set; }
        public string? VoucherData { get; set; }
        [NotMapped]
        public bool PreventStockIn { get; set; }
    }
}
