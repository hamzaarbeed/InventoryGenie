using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class EmployeeController : Controller
    {
        readonly string[] sortByOptions ={
            "Employee ID",
            "Username",
            "First Name",
            "Last Name",
            "Role"
        };
        public EmployeeController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
            ApplicationDbContext.QRoles = Employee.LoggedInEmployee.GetAllRoles();
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
            ApplicationDbContext.QEmployees = 
                Employee.LoggedInEmployee.SearchEmployees(defaultSortBy,null);
            return View(ApplicationDbContext.QEmployees);
        }

        [HttpPost]
        public IActionResult Index(string sortBy, string searchText)
        {
            ViewBag.SortByOptions = sortByOptions;
            ApplicationDbContext.QEmployees = Employee.LoggedInEmployee.SearchEmployees(sortBy, searchText);
            return View(ApplicationDbContext.QEmployees);
        }

        public IActionResult SearchResult()
        {
            ViewBag.SortByOptions = sortByOptions;
            return View(ApplicationDbContext.QEmployees);
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            Employee employee = Employee.LoggedInEmployee.GetEmployeeById(id);
            return View("Details", employee);
        }

        private void PrepareViewBagFor(string actionName)
        {
            ViewBag.Action = actionName;
            ViewBag.Roles = ApplicationDbContext.QRoles;
        }
            [HttpGet]
        public IActionResult Add()
        {
            PrepareViewBagFor("Add");
            return View("Edit", new Employee());
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Employee.LoggedInEmployee.CreateEmployee(employee);
                }catch (Exception)
                {
                    ViewBag.Msg = "This username already in use";
                    PrepareViewBagFor("Add");
                    return View("Edit", employee);
                }
                return View("Details",employee);
            }
            PrepareViewBagFor("Add");
            return View("Edit",employee);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            Employee employee = Employee.LoggedInEmployee.GetEmployeeById(id);
            PrepareViewBagFor("Edit");
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
            PrepareViewBagFor("Edit");
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
