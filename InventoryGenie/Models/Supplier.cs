using System.ComponentModel.DataAnnotations;

namespace InventoryGenie.Models
{
    public class Supplier
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter supplier name")]
        public string SupplierName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public string? OrderingInstructions { get; set; }

        public ICollection<Product> Products { get; set;}

    }
}
