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
            if (TempData["UserID"] == null)
                return RedirectToAction("Index", "Login");
            else
            {
                return View();
            }

        }

     
    }
}