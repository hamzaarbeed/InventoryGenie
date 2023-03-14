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

        public override Employee AutoGenerateUsername()
        {
            //to auto generate Username From ID
            //Employee must be added to database and commit changes to get EmployeeID
            //if GM decided to cancel adding this Employee, this dummy employee will be deleted from db
            Employee e = new Employee()
            {
                FirstName = " ",
                LastName = " ",
                UserName = " ",
                RoleId = 3,
                Password = " ",

            };
            Context.Employees.Add(e);
            Context.SaveChanges();
            //Generated username will be "E"+1000 + ID. eg ID 3 will generate E1003 
            e.UserName = "E" + (e.EmployeeID + 1000);
            e.RoleId = 0;
            e.FirstName = String.Empty;
            e.LastName = String.Empty;
            return e;
            
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
