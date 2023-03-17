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
                DbQueriesHolder.Products =
                    Employee.LoggedInEmployee.SalesManagementSearchProducts(defaultSortBy, null);
                return View(DbQueriesHolder.Products);
            }
        }

        [HttpPost]
        public IActionResult Index(string searchText, string sortBy)
        {
            ViewBag.SortByOptions = sortByOptions;
            DbQueriesHolder.Products =
                Employee.LoggedInEmployee.SalesManagementSearchProducts(sortBy, searchText);
            return View(DbQueriesHolder.Products);

        }
        [HttpGet]
        public IActionResult SearchResult()
        {
            ViewBag.SortByOptions = sortByOptions;
            return View("Index", DbQueriesHolder.Products);
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
            
            if (!DbQueriesHolder.Cart.ContainsKey(productID))
                DbQueriesHolder.Cart.Add(productID, quantityToBeAddedToCart);
            else
                DbQueriesHolder.Cart[productID] += quantityToBeAddedToCart;
            return RedirectToAction("SearchResult");
        }

        [HttpGet]
        public IActionResult ViewCart()
        {
            DbQueriesHolder.Products = new List<Product>();
            foreach(KeyValuePair<int,int> cartItem in DbQueriesHolder.Cart)
                DbQueriesHolder.Products.Add(Employee.LoggedInEmployee.GetProductByID(cartItem.Key));
            return View(new Tuple<List<Product>,Dictionary<int,int>>(DbQueriesHolder.Products, DbQueriesHolder.Cart));
        }

        [HttpPost]
        public IActionResult UpdateCart(int productID,int changeQuantityInCart)
        {
            DbQueriesHolder.Cart[productID] = changeQuantityInCart;
            return View("ViewCart", new Tuple<List<Product>, Dictionary<int, int>>(DbQueriesHolder.Products, DbQueriesHolder.Cart));
        }
    }
}
