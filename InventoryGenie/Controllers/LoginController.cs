using InventoryGenie.Models;
using InventoryGenie.Data;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class LoginController : Controller
    {
        private ApplicationDbContext context { get; set; }
        public LoginController(ApplicationDbContext ctx)
        {
            context = ctx;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View(new User()) ;
        }

        [HttpPost]
        public IActionResult Index(User user)
        {

            int count = context.Users.Where(x => x.UserName == user.UserName && x.Password == user.Password).Count();
            if (count > 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.Clear();
                return View();
            }
        }
    }
}
