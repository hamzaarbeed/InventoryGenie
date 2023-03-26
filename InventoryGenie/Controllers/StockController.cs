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
            //All types of employees has acces if LoggedInEmployee is blank then redirect to login page
            if (Employee.LoggedInEmployee == null)
                return RedirectToAction("Index", "Login");
            
            SortBy = "Product ID";
            SearchText = null;
            return RedirectToAction("Search");
            
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
            //All types of employees has acces if LoggedInEmployee is blank then redirect to login page
            if (Employee.LoggedInEmployee == null)
                return RedirectToAction("Index", "Login");

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
