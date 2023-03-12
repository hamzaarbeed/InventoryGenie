namespace InventoryGenie.Models
{
    public class OrderRecord
    {
        public int Id { get; set; }
        public DateTime OrderedOn { get; set; }
        public int QuantityOrdered { get; set; }
        public double WholesalePrice { get; set; }
        public bool IsReceived { get; set; }

        public Product Product { get; set; } = null!;
    }
}
