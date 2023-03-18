using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class SalesController : Controller
    {

        readonly string[] sortByOptions =
        {
            "Product ID",
            "Name",
            "Category",
            "Supplier",
            "Shelf Price"
        };
        public SalesController(ApplicationDbContext ctx)
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
                ViewBag.SortByOptions = sortByOptions;
                string defaultSortBy = "Product ID";
                ApplicationDbContext.QProducts =
                    Employee.LoggedInEmployee.SalesManagementSearchProducts(defaultSortBy, null);
                return View(ApplicationDbContext.QProducts);
            }
        }

        [HttpPost]
        public IActionResult Index(string searchText, string sortBy)
        {
            ViewBag.SortByOptions = sortByOptions;
            ApplicationDbContext.QProducts =
                Employee.LoggedInEmployee.SalesManagementSearchProducts(sortBy, searchText);
            return View(ApplicationDbContext.QProducts);

        }
        [HttpGet]
        public IActionResult SearchResult()
        {
            ViewBag.SortByOptions = sortByOptions;
            return View("Index", ApplicationDbContext.QProducts);
        }

        [HttpGet]
        public IActionResult ProcessTransaction()
        {
            Employee.LoggedInEmployee.ProcessTransaction();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddToCart(int productID, int quantityToBeAddedToCart)
        {
            
            if (!ApplicationDbContext.Cart.ContainsKey(productID))
                ApplicationDbContext.Cart.Add(productID, quantityToBeAddedToCart);
            else
                ApplicationDbContext.Cart[productID] += quantityToBeAddedToCart;
            return RedirectToAction("SearchResult");
        }

        [HttpGet]
        public IActionResult ViewCart()
        {
            ApplicationDbContext.QProducts = new List<Product>();
            foreach(KeyValuePair<int,int> cartItem in ApplicationDbContext.Cart)
                ApplicationDbContext.QProducts.Add(Employee.LoggedInEmployee.GetProductByID(cartItem.Key));
            return View(new Tuple<List<Product>,Dictionary<int,int>>(ApplicationDbContext.QProducts, ApplicationDbContext.Cart));
        }

        [HttpPost]
        public IActionResult UpdateCart(int productID,int changeQuantityInCart)
        {
            ApplicationDbContext.Cart[productID] = changeQuantityInCart;
            return View("ViewCart", new Tuple<List<Product>, Dictionary<int, int>>(ApplicationDbContext.QProducts, ApplicationDbContext.Cart));
        }
    }
}
