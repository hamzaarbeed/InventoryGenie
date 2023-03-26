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
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");


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
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

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
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

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
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            Employee.LoggedInEmployee.ProcessTransaction(ProductsInCart,Cart);
            ProductsInCart = new ();
            Cart = new();
            return RedirectToAction("Index");
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
