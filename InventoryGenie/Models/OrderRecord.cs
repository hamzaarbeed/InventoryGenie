using System.ComponentModel.DataAnnotations;

namespace InventoryGenie.Models
{
    public class OrderRecord
    {
        public int OrderRecordID { get; set; }
        public DateTime OrderedOn { get; set; }
        [Required]
        public int QuantityOrdered { get; set; }
        public double WholesalePrice { get; set; }
        public bool IsReceived { get; set; } = false;
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public string CategoryName { get; set; }
    }
}
