using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class SupplierController : Controller
    {
        readonly string[] sortByOptions =
        {
            "Supplier ID",
            "Name",
        };
        public SupplierController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (Employee.LoggedInEmployee.RoleId != 1 && Employee.LoggedInEmployee.RoleId !=2)
            {
                Employee.Logout();
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.SortByOptions = sortByOptions;
                string defaultSortBy = "Supplier ID";
                List<Supplier> products =
                    Employee.LoggedInEmployee.SearchSuppliers(defaultSortBy, null);
                return View(products);
            }
        }

        [HttpPost]
        public IActionResult Index(string searchText, string sortBy)
        {
            ViewBag.SortByOptions = sortByOptions;
            List<Supplier> products =
                Employee.LoggedInEmployee.SearchSuppliers(sortBy, searchText);
            return View(products);
        }
    }
}
