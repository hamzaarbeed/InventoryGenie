using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext context { get; set; }
        public HomeController(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (TempData.Peek("UserID") == null)
                return RedirectToAction("Index", "Login");
            else
            {
                //stockouts are products with quantity 0
                
                ViewBag.stockoutCount = context.Products.Where(x=>x.Quantity==0).Count();

                //lowstock are products below their minimim level not including stockouts
                ViewBag.lowstockCount = context.Products.Where(x => x.Quantity <= x.MinimumLevel).Count() - ViewBag.stockoutCount;

                return View();
            }

        }

     
    }
}