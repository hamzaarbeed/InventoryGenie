using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class SalesController : Controller
    {
        private static string? SearchText;
        private static string? SortBy;

        private static Dictionary<int, int> Cart = new();
        private static List<Product> ProductsInCart = new();

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
            SortBy = sortBy;
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
            List<Product> products =
                Employee.LoggedInEmployee.SalesManagementSearchProducts(SortBy, SearchText);
            return View("Index", products);
        }

        
        [HttpPost]
        public IActionResult AddToCart(int productID, int quantityToBeAddedToCart)
        {
            
            if (!Cart.ContainsKey(productID))
                Cart.Add(productID, quantityToBeAddedToCart);
            else
                Cart[productID] += quantityToBeAddedToCart;
            return RedirectToAction("Search");
        }

        [HttpGet]
        public IActionResult ViewCart()
        {
            //All types of employees has acces if LoggedInEmployee is blank then redirect to login page
            if (Employee.LoggedInEmployee == null)
                return RedirectToAction("Index", "Login");

            ProductsInCart = new List<Product>();
            foreach(KeyValuePair<int,int> cartItem in Cart)
                ProductsInCart.Add(Employee.LoggedInEmployee.GetProductByID(cartItem.Key));
            return View(new Tuple<List<Product>,Dictionary<int,int>>(ProductsInCart, Cart));
        }

        [HttpPost]
        public IActionResult UpdateCart(int productID,int changeQuantityInCart)
        {
            Cart[productID] = changeQuantityInCart;
            return View("ViewCart", new Tuple<List<Product>, Dictionary<int, int>>(ProductsInCart, Cart));
        }

        [HttpGet]
        public IActionResult ProcessTransaction()
        {
            //All types of employees has acces if LoggedInEmployee is blank then redirect to login page
            if (Employee.LoggedInEmployee == null)
                return RedirectToAction("Index", "Login");

            Employee.LoggedInEmployee.ProcessTransaction(ProductsInCart,Cart);
            ProductsInCart = new ();
            Cart = new();
            return RedirectToAction("Index");
        }
    }
}
