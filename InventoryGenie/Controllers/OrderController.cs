using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class OrderController : Controller
    {
        private static string? SearchText;
        private static string? SortBy;
        private static bool ShowOnlyBelowMinimumLevel =true;

        readonly string[] OrderRecordSortByOptions =
        {
            "Date & Time",
            "Product",
            "Category",
            "Supplier",
        };

        readonly string[] ProductSortByOptions =
        {
            "Product ID",
            "Product Name",
        };
        public OrderController(ApplicationDbContext ctx)
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
                SortBy = "Date & Time";
                SearchText = null;
                return RedirectToAction("Search");
            }
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
            ViewBag.SortByOptions = OrderRecordSortByOptions;
            List<OrderRecord> orderRecords = Employee.LoggedInEmployee.SearchOrderRecords(SortBy, SearchText);
            return View("Index", orderRecords);
        }

        [HttpGet]
        public IActionResult Receive(int orderRecordId)
        {
            Employee.LoggedInEmployee.ReceiveOrder(orderRecordId);
            return RedirectToAction("Search");
        }

        [HttpGet]
        public IActionResult Details(int orderRecordId)
        {
            OrderRecord orderRecord = Employee.LoggedInEmployee.GetOrderRecordByID(orderRecordId);
            return View("Details",orderRecord);
        }

        [HttpGet]
        public IActionResult PlaceOrderView()
        {
            if (Employee.LoggedInEmployee == null)
                return RedirectToAction("Index", "Login");
            else
            {
                SortBy = "Product ID";
                SearchText = null;
                ShowOnlyBelowMinimumLevel = true;
                return RedirectToAction("SearchProduct");
            }
        }

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
            ViewBag.SortByOptions = ProductSortByOptions;
            ViewBag.ShowOnlyBelowMinimumLevel = ShowOnlyBelowMinimumLevel;
            
            List<Product> products = 
                Employee.LoggedInEmployee.OrderManagementSearchProducts(SortBy, SearchText,ShowOnlyBelowMinimumLevel);
            List<int> QuantityNotReceivedCount = new List<int>();
            foreach (Product product in products)
            {
                QuantityNotReceivedCount.Add(Employee.LoggedInEmployee.GetQuantityNotReceivedForProduct(product.Name));
            }
            ViewBag.QuantityNotReceivedCount = QuantityNotReceivedCount;
            return View("PlaceOrderView", products);
        }

        [HttpPost]
        public IActionResult PlaceOrder(int productID, int orderQuantity)
        {
            Employee.LoggedInEmployee.PlaceOrder(productID, orderQuantity);
            return RedirectToAction("SearchProduct");

        }

    }
}
