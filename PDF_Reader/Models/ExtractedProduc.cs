namespace PDF_Reader.Models
{
    public class ExtractedProduct
    {
        public string Qty { get; set; }
        public string Price { get; set; }
        public string? Discount { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Vat { get; set; }
    }

}
