using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class HomeController : Controller
    {
        //pass database context to Employee, so emlpoyee can manipulate it
        public HomeController(ApplicationDbContext ctx) {
            Employee.Context = ctx;
        }


        // First Method to be called in the application
        [HttpGet]
        public IActionResult Index()
        {

            //if Logged In Employee is null it means no one logged in yet
            if (Employee.LoggedInEmployee == null)
                //redirect to login page
                return RedirectToAction("Index", "Login");
            else // if employee is logged in
            {
               //if employee is General Manager (1) or WareHouse Leader (2)
                if (Employee.LoggedInEmployee.RoleId == 1 || Employee.LoggedInEmployee.RoleId == 2)
                {
                    //get stockout and low stock in Viewbag to show on main screen (Alert)
                    //stockouts are products with quantity 0
                    ViewBag.stockoutCount = Employee.LoggedInEmployee.GetStockOut().Count;

                    //lowstock are products below their minimim level not including stockouts
                    ViewBag.lowstockCount = Employee.LoggedInEmployee.GetLowStock().Count;
                }
                return View();
            }

        }

     
    }
}