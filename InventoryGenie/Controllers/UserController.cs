using InventoryGenie.Data;
using InventoryGenie.Models;
using InventoryGenie.Models.AllEmployeesFunctions;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class UserController : Controller
    {
        public UserController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
        }

        [HttpGet]
        public IActionResult Index()
        {
            
            List<Employee> employees= Employee.LoggedInEmployee.GetAllEmployeesList();

            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            ViewBag.Roles = Employee.GetAllRoles();
            return View("Edit",new Employee());
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            if (ModelState.IsValid)
            {
                Employee.LoggedInEmployee.CreateEmployee(employee);
                return RedirectToAction("Index");
            }

            return View("Edit",employee);
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

        [HttpPost]
        public IActionResult Search(string searchText)
        {
            List<Employee> employees = Employee.LoggedInEmployee.SearchEmployees(searchText);
            return View("Index",employees);
        }
    }
}
