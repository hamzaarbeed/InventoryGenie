using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class SalesController : Controller
    {
        private static string? SearchText;
        private static string? SortBy;

        //these hold the items in the cart and the quantity of the product in the cart
        //cart is emptied after checkout or return, or at logout  
        public static Dictionary<int, int> Cart = new();
        public static List<Product> ProductsInCart =new();

        readonly string[] sortByOptions =
        {
            "Product ID",
            "Name",
            "Category",
            "Supplier",
            "Shelf Price"
        };

        //gives DbContext to Employee
        public SalesController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
        }

        //first function to be called in this controller
        [HttpGet]
        public IActionResult Index()
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            //The default settings for sortby and searchtext
            SortBy = "Product ID";
            SearchText = null;

            //after getting default settings set up redirect to HttpGet Search to show all products in Index view 
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
            ViewBag.SortBy = SortBy;
            ViewBag.SearchText = SearchText;
            return View("Index", new Tuple<List<Product>, Dictionary<int, int>>(products, Cart));
        }

        [HttpGet]
        public IActionResult ClearCart()
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");
            Cart = new();
            ProductsInCart = new();

            return RedirectToAction("Search");
        }

        [HttpPost]
        public IActionResult AddToCart(int productID, int quantityToBeAddedToCart)
        {

            if (!Cart.ContainsKey(productID))
            {
                Cart.Add(productID, quantityToBeAddedToCart);
                ProductsInCart.Add(Employee.LoggedInEmployee.GetProductByID(productID));
            }
            else
                Cart[productID] += quantityToBeAddedToCart;

            return RedirectToAction("Search");
        }

        [HttpGet]
        public IActionResult ViewCart()
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            return View(new Tuple<List<Product>,Dictionary<int,int>>(ProductsInCart, Cart));
        }

        [HttpPost]
        public IActionResult UpdateCart(int productID,int changeQuantityInCart)
        {
            Cart[productID] = changeQuantityInCart;
            return View("ViewCart", new Tuple<List<Product>, Dictionary<int, int>>(ProductsInCart, Cart));
        }

        [HttpGet]
        public IActionResult CheckOut()
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            Employee.LoggedInEmployee.CheckOut(ProductsInCart,Cart);
            ProductsInCart = new ();
            Cart = new();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Return()
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            Employee.LoggedInEmployee.Return(ProductsInCart, Cart);
            ProductsInCart = new();
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
