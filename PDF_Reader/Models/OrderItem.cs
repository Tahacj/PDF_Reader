using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XPOS.Shared.Models
{
    public class OrderItem
    {
        [Key]
        public int ID { get; set; }
        public int Order_ID { get; set; }//?
        [ForeignKey(nameof(Order_ID))]
        [JsonIgnore]
        public Order? Order { get; set; }//
        public int ProductVariant_ID { get; set; }///
        //[ForeignKey(nameof(ProductVariant_ID))]
        //public ProductVariant? ProductVariant { get; set; }
        //public int ProductVariantPriceBreak_ID { get; set; }
        //[ForeignKey(nameof(ProductVariantPriceBreak_ID))]
        //public ProductVariantPriceBreak? ProductVariantPriceBreak { get; set; }
        [NotMapped]
        public int Product_ID { get; set; }//?
        [NotMapped]
        //public Product? Product { get; set; }
        public string? Name { get; set; }// mat or product code
        public int Quantity { get; set; }//
        public int AllocatedUnits { get; set; }
        public int FOCQuantity { get; set; }
        public int StockUnit { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? SalesPrice { get; set; }//
        public decimal? GrossMargin { get; set; }
        public string? VATCode { get; set; }
        public bool Caddie { get; set; }
        public bool Web { get; set; }
        public bool Shop { get; set; }
        public bool ThirdParty { get; set; }
        public int? OrderQuantity { get; set; }
        [NotMapped]
        public string? ImageURL { get; set; }
        [NotMapped]
        public string? VATLabel { get; set; }
        [NotMapped]
        public bool VATSecondHand { get; set; }
        [NotMapped]
        //public List<ProductInstanceIdentifier>? SerialNumbers { get; set; }
        //[NotMapped]
        public decimal? VATRate { get; set; }
        [NotMapped]
        public bool DialogVisible { get; set; }
    }
}
