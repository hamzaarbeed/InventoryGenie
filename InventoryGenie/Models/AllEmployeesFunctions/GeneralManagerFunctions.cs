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
        
        
        public static void CreateEmployee(Employee employee)
        {
            Context.Employees.Add(employee);
            Context.SaveChanges();
        }



    }
}
