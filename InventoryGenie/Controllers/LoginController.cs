using InventoryGenie.Data;
using InventoryGenie.Models;
using InventoryGenie.Models.AllEmployeesFunctions;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class LoginController : Controller
    {

        public LoginController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
        }
        //first function called in Login Page
        //it will create a new temproray User that will get
        //username and password back to Login action
        [HttpGet]
        public IActionResult Index()
        {
            return View(new Employee());
        }


        //after user enter username and password inside Index view
        //it comes back to here user holds username and password only. other field are empty
        [HttpPost]
        public IActionResult Login(Employee employee)
        {
            //return user with the same username and password, if not found returns null
            EmployeeFunctions.Login(employee.UserName, employee.Password);
            
            if (Employee.employee == null)//usr was not found then it's incorrect user name and password
            {
                //clears form
                ModelState.Clear();

                //stores Msg to show this message in Login View
                ViewBag.Msg = "Incorrect User name/password";

                //direct user to Login page so the user tries again
                return View("Index");
            }
            else //if user was found
            {
                if (Employee.employee.IsTemporaryPassword == true) //if user needs to change password it directs user to ChangePassword
                {
                    return View("ChangePassword", Employee.employee);
                }
                //if user doesn't need to change password then userdata will be directed to Home page
                return RedirectToAction("Index", "Home");
            }
            
        }

        //this will hand over user to ChangePassword View
        [HttpGet]
        public IActionResult ChangePassword(Employee employee)
        {
            return View(employee);
        }

        //Post method it brings back from ChangePassword View user(username, (old)password, and ID), new password and Confirmed new password
        [HttpPost]
        public IActionResult ChangePassword(Employee employee, string newPassword, string confirmedNewPassword)
        {

            //change password if all fields are ok, if not, it returns a message 
            ViewBag.Msg = EmployeeFunctions.ChangePassword(employee.Id, newPassword, confirmedNewPassword);
            
            //if password fields are not ok
            if (ViewBag.Msg != null)
            {
                //open the same page again for another try
                View("ChangePassword",employee);
            }

            //if sucessful in changing the password. employee will be logged in
            //and go to home page( index get)
            return RedirectToAction("Index", "Home");
        }


        //logs current user out
        [HttpGet]
        public IActionResult LogOut()
        {
            //clear logged in user data
            EmployeeFunctions.Logout();
            //return to Login page
            return View("Index");
        }
        

    }
}
