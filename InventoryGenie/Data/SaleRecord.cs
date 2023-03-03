namespace InventoryGenie.Data
{
    public class SaleRecord
    {
        public DateTime SoldOn { get; set; }
        public int QuantitySold { get; set; }
        public double Cost { get; set; }
        public double SellingPrice { get; set; }
        public Product Product { get; set; } = null!;

    }
}
