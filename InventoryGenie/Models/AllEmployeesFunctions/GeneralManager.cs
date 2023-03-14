using Microsoft.EntityFrameworkCore;

namespace InventoryGenie.Models.AllEmployeesFunctions
{
    public class GeneralManager:WarehouseLeader
    {
        public override List<Employee> GetAllEmployeesList()
        {
            return Context.Employees.Include(x=>x.Role).ToList();
        }
        public override List<Employee> SearchEmployees(string searchText)
        {

            return Context.Employees.Include(x=>x.Role).Where(x => 
                x.Role.RoleName.Contains(searchText)  ||
                x.UserName.Contains(searchText) ||
                x.EmployeeID.ToString().Contains(searchText) ||
                x.FirstName.Contains(searchText) ||
                x.LastName.Contains(searchText)
                ).ToList();
        }
        
        
        public override void CreateEmployee(Employee employee)
        {
            Context.Employees.Add(employee);
            Context.SaveChanges();
        }
        public override void UpdateEmployee(Employee employee)
        {
            Context.Employees.Update(employee);
            Context.SaveChanges();
        }

        public override void DeleteEmployee(Employee employee)
        {
            Context.Employees.Remove(employee);
            Context.SaveChanges();
        }
        public override Employee GetEmployeeById(int employeeId)
        {
            return Context.Employees.Include(x => x.Role).FirstOrDefault(x => x.EmployeeID == employeeId);
        }

    }
}
