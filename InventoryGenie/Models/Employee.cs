using InventoryGenie.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InventoryGenie.Models
{
    public class Employee
    {

        public static Employee employee { get; set; }

        public static ApplicationDbContext Context { get; set; }
        public enum RoleType
        {
            General_Manager =1,
            Warehouse_Leader=2,
            Associate=3
        }
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsTemporaryPassword { get; set; } = true;

        [Required(ErrorMessage = "Please select role")]
        public RoleType Role { get; set; }



    }
}
