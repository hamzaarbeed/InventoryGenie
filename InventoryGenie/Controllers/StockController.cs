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
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

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
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            ViewBag.SortByOptions = sortByOptions;
            List<Product> products = Employee.LoggedInEmployee.StockManagementSearchProducts(SortBy, SearchText);
            ViewBag.SortBy = SortBy;
            ViewBag.SearchText = SearchText;
            return View("Index",products);
        }

        [HttpPost]
        public IActionResult Update(int newQuantity, int productID)
        {
            Employee.LoggedInEmployee.ChangeQuantityTo(newQuantity, productID);
            return RedirectToAction("Search");
        }

        // if the LoggedInEmployee is not null and role is GM (1) or WL(2) or Assocaite(3) then return true.
        // if it's not true then user will be redirected to Home.
        // Home will redirect to login if there is no logged in Employee. 
        private static bool IsAuthenticatedAndAuthorized()
        {
            return Employee.LoggedInEmployee != null && (Employee.LoggedInEmployee.RoleId == 1 ||
                Employee.LoggedInEmployee.RoleId == 2 || Employee.LoggedInEmployee.RoleId == 3);
        }

    }
}
