using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class EmployeeController : Controller
    {
        private static List<Role> Roles = new();
        private static string? SearchText;
        private static string? SortBy;
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
            Roles = Employee.LoggedInEmployee.GetAllRoles();
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (Employee.LoggedInEmployee.RoleId != 1)
            {
                Employee.Logout();
                return RedirectToAction("Index","Login");
            }
            
            SortBy = "Employee ID";
            SearchText = null;
            return RedirectToAction("Search");
        }

        [HttpPost]
        public IActionResult Search(string sortBy, string searchText)
        {
            SortBy = sortBy;
            SearchText = searchText;
            return RedirectToAction("Search");
        }

        [HttpGet]
        public IActionResult Search()
        {
            ViewBag.SortByOptions = sortByOptions;
            List < Employee > employees= Employee.LoggedInEmployee.SearchEmployees(SortBy, SearchText);
            return View("Index",employees);
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
            ViewBag.Roles = Roles;
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
        public IActionResult Edit(Employee employee, string? tempPassword)
        {
            if (ModelState.IsValid)
            {
                if (tempPassword != "" && tempPassword != null)
                {
                    employee.Password = tempPassword;
                    employee.IsTemporaryPassword = true;
                }
                else
                {
                    employee.IsTemporaryPassword = false;
                }
                Employee.LoggedInEmployee.UpdateEmployee(employee);
                return RedirectToAction("Search");
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
            return RedirectToAction("Search");
        }
    }
}
