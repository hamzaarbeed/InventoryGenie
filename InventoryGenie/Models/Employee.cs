using InventoryGenie.Data;
using System.ComponentModel.DataAnnotations;

namespace InventoryGenie.Models
{
    public class Employee
    {

        public static Employee LoggedInEmployee { get; set; }

        public static ApplicationDbContext Context { get; set; }
        public enum RoleType
        {
            General_Manager =1,
            Warehouse_Leader=2,
            Associate=3
        }
        public static string GetRoleName(RoleType role)
        {
            string roleName = role.ToString().Replace("_"," ");
            return roleName;
        }
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsTemporaryPassword { get; set; } = true;

        [Required(ErrorMessage = "Please select role")]
        public string Role { get; set; }




    }
}
