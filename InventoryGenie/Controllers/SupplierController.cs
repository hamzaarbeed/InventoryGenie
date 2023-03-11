using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class SupplierController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
