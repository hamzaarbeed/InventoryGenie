using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
