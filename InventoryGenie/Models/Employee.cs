using InventoryGenie.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InventoryGenie.Models
{
    public class Employee
    {

        public static Employee LoggedInEmployee { get; set; }

        public static ApplicationDbContext Context { get; set; }

        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsTemporaryPassword { get; set; } = true;

        [Required(ErrorMessage = "Please select a role")]
        public int RoleID { get; set; }
        [ValidateNever]
        public Role Role { get; set; } = null;


    }
}
