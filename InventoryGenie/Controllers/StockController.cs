using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;


namespace InventoryGenie.Controllers
{
    public class StockController : Controller
    {
        private static string? SearchText;
        private static string? SortBy;

        readonly string[] sortByOptions =
        {
            "Product ID",
            "Name",
            "Quantity",
            "Minimum Level"
        };
        public StockController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
        }


        [HttpGet]
        public IActionResult Index()
        {
            if (Employee.LoggedInEmployee == null)
                return RedirectToAction("Index", "Login");
            else
            {
                SortBy = "Product ID";
                SearchText = null;
                return RedirectToAction("Search");
            }
        }

        [HttpPost]
        public IActionResult Search(string searchText, string sortBy)
        {
            SortBy= sortBy;
            SearchText = searchText;
            return RedirectToAction("Search");

        }

        [HttpGet]
        public IActionResult Search()
        {
            ViewBag.SortByOptions = sortByOptions;
            List<Product> products = Employee.LoggedInEmployee.StockManagementSearchProducts(SortBy, SearchText);
            return View("Index",products);
        }

        [HttpPost]
        public IActionResult Update(int newQuantity, int productID)
        {
            Employee.LoggedInEmployee.ChangeQuantityTo(newQuantity, productID);
            return RedirectToAction("Search");
        }


    }
}
