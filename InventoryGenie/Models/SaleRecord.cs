namespace InventoryGenie.Models
{
    public class SaleRecord
    {
        public int SaleRecordID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int QuantityExchanged { get; set; }
        public double WholesalePrice { get; set; }
        public double ShelfPrice { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public string CategoryName { get; set; }


    }
}
