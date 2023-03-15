using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;


namespace InventoryGenie.Controllers
{
    public class StockController : Controller
    {
        readonly string[] sortByOptions =
        {
            "Name",
            "Product ID",
            "Quantity",
            "Minimum Level"
        };
        public StockController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
        }

        [HttpPost]
        public IActionResult Update(int newQuantity,int productID)
        {
            Employee.LoggedInEmployee.ChangeQuantityTo(newQuantity,productID);
            return RedirectToAction("Index","Stock");
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (Employee.LoggedInEmployee == null)
                return RedirectToAction("Index", "Login");
            else
            {
                ViewBag.SortByOptions = sortByOptions;
                string defaultSortBy = "Name";
                List<Product> products =
                    Employee.LoggedInEmployee.StockManagementSearchProducts(defaultSortBy, null);
                return View(products);
            }
        }

        [HttpPost]
        public IActionResult Index(string searchText,string sortBy)
        {
            ViewBag.SortByOptions = sortByOptions;
            List<Product> products =
                Employee.LoggedInEmployee.StockManagementSearchProducts(sortBy, searchText); 
            return View(products);

        }
    }
}
