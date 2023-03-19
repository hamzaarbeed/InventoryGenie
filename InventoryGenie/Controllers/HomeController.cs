using InventoryGenie.Data;
using InventoryGenie.Models;
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
                if (Employee.LoggedInEmployee.RoleId != 3)
                {
                    //stockouts are products with quantity 0
                    ViewBag.stockoutCount = Employee.LoggedInEmployee.GetStockOut().Count();

                    //lowstock are products below their minimim level not including stockouts
                    ViewBag.lowstockCount = Employee.LoggedInEmployee.GetLowStock().Count();
                }
                return View();
            }

        }

     
    }
}