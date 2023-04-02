using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InventoryGenie.Models
{
    public class Product
    {
        

        
        public int ProductID { get; set; }

        public bool IsActive { get; set; } = true;
        
        [Required(ErrorMessage = "Please enter product name")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        //CategoryID is a foreign key
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; } = null!;


        //SupplierID is foriegn key
        public int? SupplierId { get; set; } = 0;
        [ValidateNever]
        public Supplier? Supplier { get; set; } = null;

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
