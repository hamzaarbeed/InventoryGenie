using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
