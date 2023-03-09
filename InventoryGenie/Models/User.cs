using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InventoryGenie.Models
{
    public class User
    {
        public enum RoleType
        {
            General_Manager =1,
            Warehouse_Leader=2,
            Associate=3
        }

        [Range(1000,int.MaxValue)]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool ChangePassword { get; set; } = true;

        [Required(ErrorMessage = "Please select role")]
        public RoleType Role { get; set; }




    }
}
