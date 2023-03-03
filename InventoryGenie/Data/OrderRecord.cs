namespace InventoryGenie.Data
{
    public class OrderRecord
    {
        public DateTime OrderedOn { get; set; }
        public int QuantityOrdered { get; set; }
        public double Cost { get; set; }
        public bool IsReceived { get; set; }

        public Product Product { get; set; } = null!;
    }
}
