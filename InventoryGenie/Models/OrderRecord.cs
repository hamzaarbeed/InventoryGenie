namespace InventoryGenie.Models
{
    public class OrderRecord
    {
        public int OrderRecordID { get; set; }
        public DateTime OrderedOn { get; set; }
        public int QuantityOrdered { get; set; }
        public double WholesalePrice { get; set; }
        public bool IsReceived { get; set; }
        public int ProductId { get; set; }
        
    }
}
