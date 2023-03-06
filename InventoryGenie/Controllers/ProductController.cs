using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
