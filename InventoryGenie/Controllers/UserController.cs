using InventoryGenie.Data;
using InventoryGenie.Models;
using InventoryGenie.Models.AllEmployeesFunctions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace InventoryGenie.Controllers
{
    public class UserController : Controller
    {
        readonly string[] sortByOptions ={
            "Employee ID",
            "Username",
            "First Name",
            "Last Name",
            "Role"
        };
        public UserController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (Employee.LoggedInEmployee.RoleId != 1)
            {
                Employee.Logout();
                return RedirectToAction("Index","Login");
            }
            ViewBag.SortByOptions = sortByOptions;
            string defaultSortBy = "Employee ID";
            List<Employee> employees= Employee.LoggedInEmployee.SearchEmployees(defaultSortBy,null);
            return View(employees);
        }

        [HttpPost]
        public IActionResult Index(string sortBy, string searchText)
        {
            ViewBag.SortByOptions = sortByOptions;
            List<Employee> employees = Employee.LoggedInEmployee.SearchEmployees(sortBy, searchText);
            return View(employees);
        }

        

        [HttpGet]
        public IActionResult View(int id)
        {
            Employee employee = Employee.LoggedInEmployee.GetEmployeeById(id);
            return View("View", employee);
        }

        [HttpGet]
        public IActionResult Add()
        {

            ViewBag.Action = "Add";
            ViewBag.Roles = Employee.GetAllRoles();
            return View("Edit", new Employee());
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            if (ModelState.IsValid)
            {
                Employee.LoggedInEmployee.CreateEmployee(employee);
                
                return View("View",employee);
            }
            return RedirectToAction("Add");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            Employee employee = Employee.LoggedInEmployee.GetEmployeeById(id);
            ViewBag.Action = "Edit";
            ViewBag.Roles = Employee.GetAllRoles();
            return View("Edit", employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee, string tempPassword)
        {
            if (ModelState.IsValid)
            {
                if (tempPassword != "")
                {
                    employee.Password = tempPassword;
                    employee.IsTemporaryPassword = true;
                }
                Employee.LoggedInEmployee.UpdateEmployee(employee);
                return RedirectToAction("Index");
            }

            
            return View("Edit", employee);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {

            Employee employee = Employee.LoggedInEmployee.GetEmployeeById(id);
            return View("Delete", employee);
        }

        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            //Employee can't delete himself
            if(Employee.LoggedInEmployee.EmployeeID!=employee.EmployeeID)
                Employee.LoggedInEmployee.DeleteEmployee(employee);
            return RedirectToAction("Index");
        }

        
    }
}
