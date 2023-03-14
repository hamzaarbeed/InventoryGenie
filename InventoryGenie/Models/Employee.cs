using InventoryGenie.Data;
using InventoryGenie.Models.AllEmployeesFunctions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static InventoryGenie.Models.AllEmployeesFunctions.Associate;

namespace InventoryGenie.Models
{
    public class Employee
    {

        public static Employee? LoggedInEmployee;

        public static ApplicationDbContext? Context { get; set; }

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsTemporaryPassword { get; set; } = true;

        [Required(ErrorMessage = "Please select a role")]
        public int RoleID { get; set; }
        [ValidateNever]
        public Role Role { get; set; } = null;

        public static void Login(string UserName, string Password)
        {
            //finds employee with the same Username and password
            LoggedInEmployee = Context.Employees.Include(x=>x.Role).FirstOrDefault(x => x.UserName == UserName && x.Password == Password);
            var serializedEmployee = JsonConvert.SerializeObject(LoggedInEmployee);

            if (LoggedInEmployee.RoleID == 1) {
                LoggedInEmployee = JsonConvert.DeserializeObject<GeneralManager>(serializedEmployee);
            }
            else if (LoggedInEmployee.RoleID == 2)
            {
                LoggedInEmployee = LoggedInEmployee = JsonConvert.DeserializeObject<WarehouseLeader>(serializedEmployee);
            }
            else
            {
                LoggedInEmployee = LoggedInEmployee = JsonConvert.DeserializeObject<Associate>(serializedEmployee);
            }
        }

        public static List<Role> GetAllRoles()
        {
            return Context.Roles.OrderBy(x => x.RoleName).ToList();
        }

        public static void Logout()
        {
            LoggedInEmployee = null;
        }

        public static string ChangePassword(int Id, string newPassword, string confirmedNewPassword)
        {
            //if new password doesn't match confirmed new password
            if (newPassword != confirmedNewPassword)
            {
                //then save this message to be shown in ChangePassword view
                return "new password doesn't match confirmed new password";
            }

            LoggedInEmployee = Context.Employees.Include(x => x.Role).FirstOrDefault(x => x.Id == Id);//gets an employee that has that Id
            //if the new password the same as old password
            if (newPassword == LoggedInEmployee.Password)
            {
                //then save this message to be shown in ChangePassword view
                return "new password doesn't match confirmed new password";
            }


            //if all field are ok then change password
            LoggedInEmployee.Password = newPassword;
            //this user doesn't need to change password in the coming logins.
            LoggedInEmployee.IsTemporaryPassword = false;

            //save changes
            Context.SaveChanges();

            //save Login Data

            return null;



        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //Virtual functions that will be overidden
        //------------------------------------------------------------------------------------------
        
        //---------------------Associate Functions--------------------------------------------------
        public virtual List<Product> GetAllProductsList(SortProductByType sortBy)
        {
            return null;
        }

        // Might change Search to Modular where you specify to search by what
        public virtual List<Product> SearchProducts(SortProductByType sortBy, string searchText, bool byID, bool byName, bool byCategory,
            bool byDescription, bool bySupplierName, bool byQuantity, bool byMaximumLevel, bool byMinimumLevel,
            bool byWholesalePrice, bool byShelfPrice)
        {
            return null;
        }

        public virtual void ChangeQuantityTo(int newQuantity, int productID)
        {
        }

        //quantityChange can be positive(to increase Quantity) or negative (to decrease Quantity)
        public virtual void ChangeQuantityBy(int quantityChange, Product product)
        {
        }
        public virtual void ChangeQuantityBy(int quantityChange, int productID)
        {
        }
        //quantityExchanged can be positive(sold) or be negative(returned)
        public virtual void CheckOut(int quantityExchanged, int productID)
        {
        }

        //----------------------------Warehouse Leader Functions -------------------------------------
        public virtual int GetStockOutCount()
        {
            return -1;
        }
        public virtual int GetLowStockCount()
        {
            return -1;
        }

        //----------------------General Manager Functions--------------------------------------------
        public virtual List<Employee> GetAllEmployeesList()
        {
            return null;
        }
        public virtual List<Employee> SearchEmployees(string searchText)
        {
            return null;
        }

        public virtual void CreateEmployee(Employee employee)
        {
        }
        public virtual void UpdateEmployee(Employee employee)
        {
        }

        public virtual void DeleteEmployee(Employee employee)
        {
        }
        public virtual Employee GetEmployeeById(int Id)
        {
            return null;
        }


    }
}
