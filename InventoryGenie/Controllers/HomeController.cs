using InventoryGenie.Data;
using InventoryGenie.Models;
using InventoryGenie.Models.AllEmployeesFunctions;

using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ApplicationDbContext ctx) {
            Employee.Context = ctx;
        }
        [HttpGet]
        public IActionResult Index()
        {

            if (Employee.LoggedInEmployee == null)
                return RedirectToAction("Index", "Login");
            else
            {
                //stockouts are products with quantity 0
                ViewBag.stockoutCount = WarehouseLeaderFunctions.getStockOutCount();

                //lowstock are products below their minimim level not including stockouts
                ViewBag.lowstockCount = WarehouseLeaderFunctions.getLowStockCount();

                return View();
            }

        }

     
    }
}