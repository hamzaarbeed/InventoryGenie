﻿using System.ComponentModel.DataAnnotations;

namespace InventoryGenie.Models
{
    public class Supplier
    {

        public int SupplierID { get; set; }

        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Please enter supplier name")]
        public string? SupplierName { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public string? OrderingInstructions { get; set; }

        public ICollection<Product>? Products { get; set;}

    }
}
