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

        [HttpPost]
        public IActionResult ChangePassword(User user,int userID, string oldPassword, string newPassword, string confirmedNewPassword)
        {
            user = context.Users.Find(userID);
            if (user.Password != oldPassword) {
                ViewBag.Msg = "old password doesn't match record";
                return View(user);
            }
            if (newPassword != confirmedNewPassword)
            {
                ViewBag.Msg = "new password doesn't match confirmed new password";
                return View(user);
            }
            user.Password = newPassword;
            user.ChangePassword = false;
            context.SaveChanges();
            return RedirectToAction("Index","Login");
        }
        [HttpGet]
        public IActionResult ChangePassword(User user)
        { 
            return View(user);
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var usr = context.Users.FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
            if (usr != null)
            {
                if (usr.ChangePassword == true)
                {
                    ViewBag.ChangePassword = true;
                    return View("ChangePassword", usr);
                }
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
                TempData["Msg"] = "Incorrect User name/password";
                return RedirectToAction ("Index","Login");
            }
        }
        public IActionResult Index()
        {
            return View(new User());
        }
    }
}
