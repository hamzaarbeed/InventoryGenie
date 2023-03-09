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


        //first function called in Login Page
        //it will create a new temproray User that will get
        //username and password back to Login action
        [HttpGet]
        public IActionResult Index()
        {
            return View(new User());
        }


        //after user enter username and password inside Index view
        //it comes back to here user holds username and password only. other field are empty

        [HttpPost]
        public IActionResult Login(User user)
        {
            //find usr with the same username and password
            user = context.Users.FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
            
            
            if (user != null) //if user was found
            {
                if (user.ChangePassword == true) //if user needs to change password it directs user to ChangePassword
                {

                    return View("ChangePassword", user);
                }

                //if user doesn't need to change password then userdata will be saved
                //in TempData, so they are accessable from all the different conrollers and view
                saveLoginDate(user);
                return RedirectToAction("Index", "Home");
            }
            else//usr was not found then it's incorrect user name and password
            {
                //clears form
                ModelState.Clear();

                //stored to show this message in Login View
                TempData["Msg"] = "Incorrect User name/password";

                //direct user to Login page so the user tries again
                // we can write return view("index"); it will do the same 
                return RedirectToAction("Index", "Login");
            }
        }

        //this will hand over user to ChangePassword View
        [HttpGet]
        public IActionResult ChangePassword(User user)
        {
            return View(user);
        }

        //Post method it brings back from ChangePassword View user(username, (old)password, and ID), new password and Confirmed new password
        [HttpPost]
        public IActionResult ChangePassword(User user, string newPassword, string confirmedNewPassword)
        {
            int y= user.Id;
            //This to get back a user with all the fields
            user = context.Users.Find(user.Id);//User before this line had only username, (old)password, and ID

            //if new password doesn't match confirmed new password
            if (newPassword != confirmedNewPassword)
            {
                //then save this message to be shown in ChangePassword view
                ViewBag.Msg = "new password doesn't match confirmed new password";
                return View(user);
            }

            //if new password matches confirmed new password then
            //change the password
            user.Password = newPassword;
            //this user doesn't need to change password in the coming logins.
            user.ChangePassword = false;

            //save changes
            context.SaveChanges();

            // save logged in user data
            saveLoginDate(user);
            // go to home page( index get)
            return RedirectToAction("Index", "Home");
        }


        //logs current user out
        [HttpGet]
        public IActionResult LogOut()
        {
            //clear logged in user data
            TempData.Clear();

            //return to Login page
            return View("Index");
        }
        
        private void saveLoginDate(User user)
        {
            // save logged in user data
            TempData["UserID"] = user.Id;
            TempData["UserRole"] = user.Role;
            TempData["UserFullName"] = user.FirstName + " " + user.LastName;
            TempData["UserRoleName"] = user.Role.ToString().Replace("_"," ");
        }

    }
}
