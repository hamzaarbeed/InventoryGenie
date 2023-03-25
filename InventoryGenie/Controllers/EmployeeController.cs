using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    // contoller to CRUD employees
    public class EmployeeController : Controller
    {
        // to store all roles names in this list. It will save sometimes going to db to retrieve that
        private static List<Role> Roles = new();

        //search text will be blank and Sortby will be by ID once Index page is opened.
        //after that, once the user search one time SearchText and Sortby will be changed to open the page again with the same
        //query result  
        private static string? SearchText;
        private static string? SortBy;

        //These are used to be put in ViewBag to show in select menu
        readonly string[] sortByOptions ={
            "Employee ID",
            "Username",
            "First Name",
            "Last Name",
            "Role"
        };

        // gives Database context to Employee so Employee can manipulate DB
        public EmployeeController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
            Roles = Employee.LoggedInEmployee.GetAllRoles();
        }

        //Main page in Employee controller
        [HttpGet]
        public IActionResult Index()
        {
            // if the role is not GM (1) then redirect to Home. Home will redirect to login if there is no logged in Employee. 
            if (Employee.LoggedInEmployee.RoleId != 1)
            {
                return RedirectToAction("Index","Home");
            }
            
            //default field for query
            // This will show all employees sorted by Employee ID
            SortBy = "Employee ID";
            SearchText = null;
            //Redirect to Search [httpGet]
            return RedirectToAction("Search");
        }

        [HttpGet]
        public IActionResult Search()
        {
            // Store sortByOptions in ViewBag
            ViewBag.SortByOptions = sortByOptions;
            // gets list of Employees sorted by SortBy and Include the text in SearchText.
            // SearchText will search all columns shown on screen except the ones with numbers.
            List<Employee> employees = Employee.LoggedInEmployee.SearchEmployees(SortBy, SearchText);
            return View("Index", employees);
        }

        // Gets sortBy and searchText passed from Index view
        [HttpPost]
        public IActionResult Search(string sortBy, string searchText)
        {
            SortBy = sortBy;
            SearchText = searchText;
            //redirects to HttpGet [Search]
            return RedirectToAction("Search");
        }

        

        // shows details about employee
        [HttpGet]
        public IActionResult Details(int id)
        {
            Employee employee = Employee.LoggedInEmployee.GetEmployeeById(id);
            return View("Details", employee);
        }

        // Prepares ViewBag with Action name either Edit or Add
        // Also with roles for the select menu
        [NonAction]
        private void PrepareViewBagFor(string actionName)
        {
            ViewBag.Action = actionName;
            ViewBag.Roles = Roles;
        }

        // Adding user user
        [HttpGet]
        public IActionResult Add()
        {
            PrepareViewBagFor("Add");
            //Start Edit view with new Employee
            return View("Edit", new Employee());
        }

        //Adding new user
        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            //if all fields are ok
            if (ModelState.IsValid)
            {
                // CreateEmployee will add employee to database
                // then get the auto generated ID and use it to make the auto generated username
                Employee.LoggedInEmployee.CreateEmployee(employee);

                // show new employee details including the auto generated username
                return View("Details", employee);
            }
            else
            {
                // if fields are not make the user enter fields again 
                PrepareViewBagFor("Add");
                return View("Edit", employee);
            }
        }

        //edit an employee
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // gets employee from database using id
            Employee employee = Employee.LoggedInEmployee.GetEmployeeById(id);
            //Prepare ViewBag for edit window with all roles
            PrepareViewBagFor("Edit");
            return View("Edit", employee);
        }

        // edit an employee
        [HttpPost]
        public IActionResult Edit(Employee employee, string? tempPassword)
        {
            //if fields are ok
            if (ModelState.IsValid)
            {
                //if temp password field is not null. it means that General Manager wants the user to change their password
                //General Manager will provide the employee with the temporary password that will be changed in next log in.

                if (tempPassword != "" && tempPassword != null)
                {
                    //store temporary password in password
                    employee.Password = tempPassword;
                    // Stores boolean that is used to know if password in temporary or not
                    employee.IsTemporaryPassword = true;
                }
                else
                {
                    //if tempPassword is null or "" then GM wants the user to keep using the existing password.
                    //IsTemporaryPassword is false to not prompt employee to change password.
                    employee.IsTemporaryPassword = false;
                }

                //update employee in database
                Employee.LoggedInEmployee.UpdateEmployee(employee);
                //goes back to employees search screen with the same previous search settings
                return RedirectToAction("Search");
            }
            else
            { // if fields are not make the user enter fields again 
                PrepareViewBagFor("Edit");
                return View("Edit", employee);
            }
        }


        // deletes an employee 
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // get employee by id from database
            Employee employee = Employee.LoggedInEmployee.GetEmployeeById(id);
            //views it and asks the user to confirm deletion
            return View("Delete", employee);
        }

        // deletes an employee
        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            //Employee can't delete himself
            if(Employee.LoggedInEmployee.EmployeeID!=employee.EmployeeID)
                Employee.LoggedInEmployee.DeleteEmployee(employee);
            // Either deletion was successful or not redirect to employees search screen with the same previous search settings
            return RedirectToAction("Search");
        }
    }
}
