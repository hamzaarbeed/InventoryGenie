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
            return RedirectToAction("SearchResult");
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (Employee.LoggedInEmployee == null)
                return RedirectToAction("Index", "Login");
            else
            {
                ViewBag.SortByOptions = sortByOptions;
                string defaultSortBy = "Product ID";
                ApplicationDbContext.QProducts = 
                    Employee.LoggedInEmployee.StockManagementSearchProducts(defaultSortBy, null);
                return View(ApplicationDbContext.QProducts);
            }
        }

        [HttpGet]
        public IActionResult SearchResult()
        {
            ViewBag.SortByOptions = sortByOptions;
            return View(ApplicationDbContext.QProducts);
        }


        [HttpPost]
        public IActionResult Index(string searchText,string sortBy)
        {
            ViewBag.SortByOptions = sortByOptions;
            ApplicationDbContext.QProducts = 
                Employee.LoggedInEmployee.StockManagementSearchProducts(sortBy, searchText);
            return View(ApplicationDbContext.QProducts);

        }
    }
}
