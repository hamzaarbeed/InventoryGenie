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

            var usr = context.Users.FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
            if (usr != null)
            {
                TempData["UserID"] = usr.Id;
                TempData["UserRole"] = usr.RoleId;
                TempData["UserFullName"] = usr.FirstName + " " + usr.LastName;
                TempData["UserRoleName"] = ((Role)context.Roles.FirstOrDefault(x => x.RoleId == usr.RoleId)).RoleName;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //clears form
                ModelState.Clear();
                ViewBag.Msg = "Incorrect User name/password";
                return View(new User());
            }
        }
    }
}
