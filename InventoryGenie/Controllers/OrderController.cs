using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{

    //Order Controller to place and receive orders
    //only General Manager and Warehouse leaders have access to this
    //Everyone else will be directed to home
    public class OrderController : Controller
    {
        //search text will be blank and Sortby will be by Date & Time once Index page is opened.
        //after that, once the user search one time, SearchText and Sortby will be changed. To open the page again with the same
        //query result
        private static string? SearchText;
        private static string? SortBy;

        //this is used for PlaceOrderView. It helps to show products that needs to be ordered which are the ones below minimum level 
        private static bool ShowOnlyBelowMinimumLevel =true;

        //These are orderby options in the Orders Records page
        //This page will allow employee to receive the orders and view order details 
        readonly string[] OrderRecordSortByOptions =
        {
            "Date & Time",
            "Product",
            "Category",
            "Supplier",
        };

        //These are orderby options in the Place Order View.
        readonly string[] ProductSortByOptions =
        {
            "Product ID",
            "Product Name",
        };

        //pass database context to Employee, so employee can manipulate it
        public OrderController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
        }



        [HttpGet]
        public IActionResult Index()
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            // sets sortby to the default Date & Time and searchText null
            // This will return all order Records sorted from the newest to the oldest
            SortBy = "Date & Time";
            SearchText = null;
            // redirect to Search method [HttpGet]
            return RedirectToAction("Search");
        }


        //After searchText and sortBy was posted from Index View it saves them in the static SortBy and SearchText
        [HttpPost]
        public IActionResult Search(string searchText, string sortBy)
        {

            SortBy = sortBy;
            SearchText = searchText;
            // redirect to Search method [HttpGet]
            return RedirectToAction("Search");

        }

        //It Gets list of orders Records based on the static SearchBy and SortBy
        //and view it in Index View
        [HttpGet]
        public IActionResult Search()
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            ViewBag.SortByOptions = OrderRecordSortByOptions;
            List<OrderRecord> orderRecords = Employee.LoggedInEmployee.SearchOrderRecords(SortBy, SearchText);
            ViewBag.SortBy = SortBy;
            ViewBag.SearchText = SearchText;
            return View("Index", orderRecords);
        }

        [HttpGet]
        public IActionResult Receive(int orderRecordId)
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");


            OrderRecord orderRecord = Employee.LoggedInEmployee.GetOrderRecordByID(orderRecordId);
            ViewBag.Action = "Confirm";
            return View("Details",orderRecord);
        }

        [HttpPost]
        public IActionResult Receive(OrderRecord orderRecord)
        {
            Employee.LoggedInEmployee.ReceiveOrder(orderRecord);
            return RedirectToAction("Search");
        }

        [HttpGet]
        public IActionResult Details(int orderRecordId)
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            ViewBag.Action = "Details";
            // Gets details of an order and show the detals in Details View
            OrderRecord orderRecord = Employee.LoggedInEmployee.GetOrderRecordByID(orderRecordId);
            return View("Details",orderRecord);
        }


        [HttpGet]
        public IActionResult PlaceOrderView()
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            //sets the static SortBy, SearchText, and ShowOnlyBelowMinimumLevel to their default values
            SortBy = "Product ID";
            SearchText = null;
            ShowOnlyBelowMinimumLevel = true;
            return RedirectToAction("SearchProduct");
            
        }


        //This will receive the posted searchText, sortBy, and showOnlyBelowMinimumLevel,
        //and save their content into their static equivelent 
        [HttpPost]
        public IActionResult SearchProduct(string searchText, string sortBy,bool showOnlyBelowMinimumLevel)
        {

            SortBy = sortBy;
            SearchText = searchText;
            ShowOnlyBelowMinimumLevel=showOnlyBelowMinimumLevel;
            return RedirectToAction("SearchProduct");

        }

        [HttpGet]
        public IActionResult SearchProduct()
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            //prepare viewbag for sort by select menu and the check box show only below minimum level
            ViewBag.SortByOptions = ProductSortByOptions;
            ViewBag.ShowOnlyBelowMinimumLevel = ShowOnlyBelowMinimumLevel;

            //Gets products list searched by SearchText, sorted by SortBy and will take ShowOnlyBelowMinimumLevel value in consideration
            List<Product> products = 
                Employee.LoggedInEmployee.OrderManagementSearchProducts(SortBy, SearchText,ShowOnlyBelowMinimumLevel);

            //Gets Totsl Quantity Not Received yet for each product that resulted from the previous query.
            //This will help in placing orders to take these in consideration and will help in deciding Recommended Quantity to Order
            List<int> QuantityNotReceivedCount = new List<int>();
            foreach (Product product in products)
            {
                QuantityNotReceivedCount.Add(Employee.LoggedInEmployee.GetQuantityNotReceivedForProduct(product.Name));
            }
            ViewBag.QuantityNotReceivedCount = QuantityNotReceivedCount;
            ViewBag.SortBy = SortBy;
            ViewBag.SearchText = SearchText;
            return View("PlaceOrderView", products);
        }


        [HttpPost]
        public IActionResult PlaceOrder(int productID, int orderQuantity)
        {
            // This will create Order Record in Data base
            Employee.LoggedInEmployee.PlaceOrder(productID, orderQuantity);
            return RedirectToAction("SearchProduct");

        }

        // if the LoggedInEmployee is not null and role is GM (1) or WL(2) then return true.
        // if it's not true then user will be redirected to Home.
        // Home will redirect to login if there is no logged in Employee. 
        private static bool IsAuthenticatedAndAuthorized()
        {
            return Employee.LoggedInEmployee != null && (Employee.LoggedInEmployee.RoleId == 1 || Employee.LoggedInEmployee.RoleId == 2);
        }

    }
}
