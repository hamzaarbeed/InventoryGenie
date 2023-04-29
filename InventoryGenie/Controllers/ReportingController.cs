using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class ReportingController : Controller
    {
        //gives DbContext to Employee
        public ReportingController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
        }

        //first funciton to be called in this controller
        [HttpGet]
        public IActionResult Index()
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            return View();
        }

        //This generates SalesReport and open Sales report view
        [HttpPost]
        public IActionResult SalesReport(string from,string to)
        {
            List<SaleRecord> salesReport = Employee.LoggedInEmployee.GenerateSalesReport(from, to);
            ViewBag.SalesFromDate = from;
            ViewBag.SalesToDate = to;

            return View(salesReport);
        }

        //This generates OrdersReport and open Orders report view
        [HttpPost]
        public IActionResult OrdersReport(string from, string to)
        {
            List<OrderRecord> OrdersReport = Employee.LoggedInEmployee.GenerateOrdersReport(from, to);
            ViewBag.OrdersFromDate = from;
            ViewBag.OrdersToDate = to;
            return View(OrdersReport);
        }

        //This generates Quantity and open Quantity report view
        [HttpGet]
        public IActionResult QuantityReport()
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");


            List<Product> products =
                Employee.LoggedInEmployee.OrderManagementSearchProducts(null, null, true);
            List<int> QuantityNotReceivedCount = new List<int>();
            foreach (Product product in products)
            {
                QuantityNotReceivedCount.Add(Employee.LoggedInEmployee.GetQuantityNotReceivedForProduct(product.Name));
            }
            ViewBag.QuantityNotReceivedCount = QuantityNotReceivedCount;
            return View(products);
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
