using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InventoryGenie.Models
{
    public class Product
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter product name")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? Category { get; set; }

        [Required(ErrorMessage = "Please choose supplier, if not in list add new supplier")]

        //SupplierID is foriegn key
        public int SupplierID { get; set; }
        [ValidateNever]
        public Supplier Supplier { get; set; } = null;

        public int Quantity { get; set; } = 0;

        [Required(ErrorMessage = "Please enter minimum level")]
        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid integer")]
        public int MinimumLevel { get; set; }

        [Required(ErrorMessage = "Please enter maximum level")]
        [Range(1, int.MaxValue, ErrorMessage = "Enter a valid integer")]
        public int MaximumLevel { get; set; }


        [Required(ErrorMessage = "Please Enter cost for the product")]
        [Range(0, double.MaxValue, ErrorMessage = "Enter a valid decimal")]
        public double WholesalePrice { get; set; }

        [Required(ErrorMessage = "Please Enter selling price for the product")]
        [Range(0, double.MaxValue, ErrorMessage = "Enter a valid decimal")]
        public double ShelfPrice { get; set; }

    }
}
