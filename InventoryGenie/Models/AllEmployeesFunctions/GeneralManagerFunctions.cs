using Microsoft.EntityFrameworkCore;

namespace InventoryGenie.Models.AllEmployeesFunctions
{
    public class GeneralManager:WarehouseLeaderFunctions
    {
        public static List<Employee> GetAllEmployeesList()
        {
            return Context.Employees.Include(x=>x.Role).ToList();
        }
        public static List<Employee> SearchEmployees(string searchText)
        {

            return Context.Employees.Include(x=>x.Role).Where(x => 
                x.Role.RoleName.Contains(searchText)  ||
                x.UserName.Contains(searchText) ||
                x.Id.ToString().Contains(searchText) ||
                x.FirstName.Contains(searchText) ||
                x.LastName.Contains(searchText)
                ).ToList();
        }
        
        
        public static void CreateEmployee(string firstName,string lastName,int roleID,string username, string tempPassword)
        {
            Employee employee = new Employee
            {
                UserName = username,
                FirstName = firstName,
                LastName = lastName,
                RoleID = roleID,
                Password = tempPassword
            };

            Context.Employees.Add(employee);
            Context.SaveChanges();
            /*//to auto generate id
            //generate username from id
            employee.UserName = "E" + (1000 + employee.Id);
            Context.SaveChanges();
            */
        }



    }
}
