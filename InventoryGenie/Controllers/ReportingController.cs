using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class ReportingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
