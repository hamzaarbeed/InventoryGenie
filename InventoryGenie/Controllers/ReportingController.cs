using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace InventoryGenie.Controllers
{
    public class ReportingController : Controller
    {

        public ReportingController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SalesReport(string from,string to)
        {
            List<SaleRecord> salesReport = Employee.LoggedInEmployee.GenerateSalesReport(from, to);
            return View(salesReport);
        }

        [HttpPost]
        public IActionResult OrdersReport(string from, string to)
        {
            List<OrderRecord> OrdersReport = Employee.LoggedInEmployee.GenerateOrdersReport(from, to);
            return View(OrdersReport);
        }
        
        public IActionResult QuantityReport()
        {
            
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
    }
}
