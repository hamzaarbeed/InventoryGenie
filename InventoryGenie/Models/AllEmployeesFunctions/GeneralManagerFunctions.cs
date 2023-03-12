﻿namespace InventoryGenie.Models.AllEmployeesFunctions
{
    public class GeneralManager:WarehouseLeaderFunctions
    {
        public static List<Employee> GetAllEmployeesList()
        {
            return Context.Employees.ToList();
        }
        public static List<Employee> SearchEmployees(string searchText)
        {

            return Context.Employees.Where(x => 
                x.Role.Contains(searchText)  ||
                x.UserName.Contains(searchText) ||
                x.Id.ToString().Contains(searchText) ||
                x.FirstName.Contains(searchText) ||
                x.LastName.Contains(searchText)
                ).ToList();
        }
        
        
        public static void CreateEmployee(string firstName,string lastName,RoleType role,string username, string tempPassword)
        {
            Employee employee = new Employee
            {
                UserName = username,
                FirstName = firstName,
                LastName = lastName,
                Role = GetRoleName(role),
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
