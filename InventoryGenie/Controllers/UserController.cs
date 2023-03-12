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
            
            List<Employee> employees= GeneralManager.GetAllEmployeesList();

            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            ViewBag.Roles = EmployeeFunctions.GetAllRoles();
            return View("Edit",new Employee());
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            if (ModelState.IsValid)
            {
                GeneralManager.CreateEmployee(employee);
                return RedirectToAction("Index");
            }

            return View("Edit",employee);
        }

        [HttpPost]
        public IActionResult Search(string searchText)
        {
            List<Employee> employees = GeneralManager.SearchEmployees(searchText);
            return View("Index",employees);
        }
    }
}
