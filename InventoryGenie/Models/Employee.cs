using InventoryGenie.Data;
using InventoryGenie.Models.AllEmployeesFunctions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace InventoryGenie.Models
{
    public class Employee
    {
        
        public static Employee? LoggedInEmployee;

        public static ApplicationDbContext? Context { get; set; }

        
        public static List<Employee> Employees { get; set; } = new List<Employee>();

        public int EmployeeID { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsTemporaryPassword { get; set; } = true;

        [Required(ErrorMessage = "Please select a role")]
        public int RoleId { get; set; }
        [ValidateNever]
        public Role Role { get; set; } = null!;

        
        public static void Login(string UserName, string Password)
        {
            //finds employee with the same Username and password
            LoggedInEmployee = Context.Employees.Include(x=>x.Role).FirstOrDefault(x => x.UserName == UserName && x.Password == Password);
            //this convert LoggedInEmployee from Employee to General Manager, for example.
            if (LoggedInEmployee != null)
                CastDownFromEmployeeToRole();
        }

        protected static void CastDownFromEmployeeToRole()
        {
            var serializedEmployee = JsonConvert.SerializeObject(LoggedInEmployee);

            if (LoggedInEmployee.RoleId == 1)
            {
                LoggedInEmployee = JsonConvert.DeserializeObject<GeneralManager>(serializedEmployee);
            }
            else if (LoggedInEmployee.RoleId == 2)
            {
                LoggedInEmployee = LoggedInEmployee = JsonConvert.DeserializeObject<WarehouseLeader>(serializedEmployee);
            }
            else
            {
                LoggedInEmployee = LoggedInEmployee = JsonConvert.DeserializeObject<Associate>(serializedEmployee);
            }
        }

        public static void Logout()
        {
            LoggedInEmployee = null;
        }

        public static string ChangePassword(int Employeeid, string newPassword, string confirmedNewPassword)
        {
            //if new password doesn't match confirmed new password
            if (newPassword != confirmedNewPassword)
            {
                //then save this message to be shown in ChangePassword view
                return "new password doesn't match confirmed new password";
            }

            LoggedInEmployee = Context.Employees.Include(x => x.Role).FirstOrDefault(x => x.EmployeeID == Employeeid);//gets an employee that has that Id
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

            //this convert LoggedInEmployee from Employee to General Manager, Warehouse leader, or associate
            CastDownFromEmployeeToRole();

            // no error messages
            return null;
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //Virtual functions that will be overridden, functions that are not overridden will not do
        //any action and will throw exception. If the object created overrides a function, it means
        //that this object is authorized to use this function
        //------------------------------------------------------------------------------------------

        //---------------------Associate Functions--------------------------------------------------


        public virtual List<Product> StockManagementSearchProducts(string sortBy, string searchText)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        public virtual List<Product> SalesManagementSearchProducts(string sortBy, string searchText)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        public virtual void ChangeQuantityTo(int newQuantity, int productID)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        //quantityExchanged can be positive(sold) or be negative(returned)
        public virtual void ProcessTransaction(List<Product> productsInCart, Dictionary<int, int> cart)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }
        public virtual Product GetProductByID(int productID)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        //----------------------------Warehouse Leader Functions -------------------------------------
        public virtual List<Product> GetStockOut()
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }
        public virtual List<Product> GetLowStock()
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }
        public virtual List<Supplier> SearchSuppliers(string sortBy, string searchText)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }
        public virtual Supplier GetSupplierByID(int supplierID)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }
        public virtual void CreateSupplier(Supplier supplierID)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }
        public virtual void UpdateSupplier(Supplier supplierID)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        public virtual void DeleteSupplier(Supplier supplier)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        public virtual List<Product> ProductManagementSearchProducts(string sortBy, string searchText)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        public virtual List<Category> GetAllCategories()
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        public virtual List<Supplier> GetAllSuppliers()
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        public virtual void CreateProduct(Product productID)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        public virtual void UpdateProduct(Product productID)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        public virtual void DeleteProduct(Product product)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        //----------------------General Manager Functions--------------------------------------------

        public virtual List<Employee> SearchEmployees(string sortBy, string searchText)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        public virtual List<Role> GetAllRoles()
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        public virtual void CreateEmployee(Employee employee)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }
        public virtual void UpdateEmployee(Employee employee)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }

        public virtual void DeleteEmployee(Employee employee)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }
        public virtual Employee GetEmployeeById(int Id)
        {
            throw new Exception("Unauthorized Access. Can't perform this function");
        }
    }
}
