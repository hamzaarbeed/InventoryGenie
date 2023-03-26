using Microsoft.EntityFrameworkCore;

namespace InventoryGenie.Models.AllEmployees
{
    public class GeneralManager:WarehouseLeader
    {

        public override List<Employee> SearchEmployees(string sortBy, string searchText)
        {
            IQueryable<Employee> query;
            if (searchText != null)
            {
                query = Context.Employees.Include(x => x.Role).Where(x =>
                    x.Role.RoleName.Contains(searchText) ||
                    x.UserName.Contains(searchText) ||
                    x.EmployeeID.ToString().Contains(searchText) ||
                    x.FirstName.Contains(searchText) ||
                    x.LastName.Contains(searchText)
                );
            }
            else
            {
                query = Context.Employees.Include(x => x.Role);
            }
            switch (sortBy)
            {
                default:
                case "Employee ID":
                    return query.OrderBy(x => x.EmployeeID).ToList();
                case "Username":
                    return query.OrderBy(x => x.UserName).ToList();
                case "First Name":
                    return query.OrderBy(x => x.FirstName).ToList();
                case "Last Name":
                    return query.OrderBy(x => x.LastName).ToList();
                case "Role":
                    return query.OrderBy(x => x.Role.RoleName).ToList();
            }
        }

        public override List<Role> GetAllRoles()
        {
            return Context.Roles.OrderBy(x => x.RoleName).ToList();
        }

        public override void CreateEmployee(Employee employee)
        {
            employee.UserName = "jbivuvukbnuhquheqhweuidqd12341e31ws123";
            Context.Employees.Add(employee);
            Context.SaveChanges();
            //this will generate ID which will be used to create Username

            //this will load Role object and attach it to employee, so employee.Role will not be null
            employee = Context.Employees.Include(x => x.Role).FirstOrDefault(x => x.EmployeeID== employee.EmployeeID);
            
            //username will be generated automatically. eg ID = 3 username is e1003. which will be unique too
            employee.UserName = "e" + (1000 + employee.EmployeeID);
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
