using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class LoginController : Controller
    {
        //move database context to Employee so that employee can manipulate database
        public LoginController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
        }

        //default action called in Login Controller
        // it will make new employee object to prompt employee to enter username and password
        // other fields are not necessary
        [HttpGet]
        public IActionResult Index()
        {
            return View(new Employee());
        }


        //after employee enter username and password inside Index view
        //it comes back to here employee holds username and password only. other field are empty
        [HttpPost]
        public IActionResult Login(Employee employee)
        {
            //will search for employee with the same username and password, and save it in LoggedInEmployee if not found it saves null
            Employee.Login(employee.UserName, employee.Password);
            
            if (Employee.LoggedInEmployee == null)//if LoggedInEmployee is null it means employee
                                                  //was not found then it's incorrect username and password
            {
                //clears form
                ModelState.Clear();

                //stores Msg to show this message in Login View
                ViewBag.Msg = "Incorrect Username/password";

                //direct employee to Login page so the employee tries again
                return View("Index");
            }
            else //if employee was found
            {
                if (Employee.LoggedInEmployee.IsTemporaryPassword == true) //if employee needs to change password
                                                                           //it directs employee to ChangePassword
                {
                    return View("ChangePassword", Employee.LoggedInEmployee);
                }
                //if employee doesn't need to change password then empoloyee will be directed to Home page
                return RedirectToAction("Index", "Home");
            }
            
        }

        //this will prompt employee to ChangePassword View
        [HttpGet]
        public IActionResult ChangePassword(Employee LoggedInEmployee)
        {
            return View(LoggedInEmployee);
        }

        //Post method it brings back from ChangePassword View employee(username, (old)password, and ID),
        //new password and Confirmed new password
        [HttpPost]
        public IActionResult ChangePassword(Employee LoggedInEmployee, string newPassword, string confirmedNewPassword)
        {

            //change password if all fields are ok, if not, it returns a message 
            ViewBag.Msg = Employee.ChangePassword(LoggedInEmployee.EmployeeID, newPassword, confirmedNewPassword);
            
            //if password fields are not ok because not null msg means there is something wrong with the fields
            if (ViewBag.Msg != null)
            {
                //open the same page again for another try
                View("ChangePassword", LoggedInEmployee);
            }

            //if sucessful in changing the password. employee will be logged in
            //and go to home page( index get)
            return RedirectToAction("Index", "Home");
        }


        //logs current employee out
        [HttpGet]
        public IActionResult LogOut()
        {
            //clear logged in employee data
            Employee.Logout();
            //return to Login page
            return View("Index");
        }
        

    }
}
