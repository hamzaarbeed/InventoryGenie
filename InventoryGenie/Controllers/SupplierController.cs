using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class SupplierController : Controller
    {
        readonly string[] sortByOptions =
        {
            "Supplier ID",
            "Name",
        };
        public SupplierController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (Employee.LoggedInEmployee.RoleId != 1 && Employee.LoggedInEmployee.RoleId !=2)
            {
                Employee.Logout();
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.SortByOptions = sortByOptions;
                string defaultSortBy = "Supplier ID";
                ApplicationDbContext.QSuppliers =
                    Employee.LoggedInEmployee.SearchSuppliers(defaultSortBy, null);
                return View(ApplicationDbContext.QSuppliers);
            }
        }

        [HttpPost]
        public IActionResult Index(string searchText, string sortBy)
        {
            ViewBag.SortByOptions = sortByOptions;
            ApplicationDbContext.QSuppliers =
                Employee.LoggedInEmployee.SearchSuppliers(sortBy, searchText);
            return View(ApplicationDbContext.QSuppliers);
        }

        [HttpGet]
        public IActionResult SearchResult()
        {
            ViewBag.SortByOptions = sortByOptions;
            return View(ApplicationDbContext.QSuppliers);
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            Supplier supplier=Employee.LoggedInEmployee.GetSupplierByID(id);
            return View(supplier);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit", new Supplier());
        }

        [HttpPost]
        public IActionResult Add(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Employee.LoggedInEmployee.CreateSupplier(supplier);
                }catch (Exception) { 
                    ViewBag.Msg = "This supplier name already in use";
                    ViewBag.Action = "Add";
                    return View("Edit", supplier);
                }
                return View("Details", supplier);
            }
            ViewBag.Action = "Add";
            return View("Edit",supplier);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Supplier supplier = Employee.LoggedInEmployee.GetSupplierByID(id);
            ViewBag.Action = "Edit";
            return View("Edit", supplier);
        }

        [HttpPost]
        public IActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                Employee.LoggedInEmployee.UpdateSupplier(supplier);
                return RedirectToAction("Index");
            }

            ViewBag.Action = "Edit";
            return View("Edit", supplier);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {

            Supplier supplier = Employee.LoggedInEmployee.GetSupplierByID(id);
            return View("Delete", supplier);
        }

        [HttpPost]
        public IActionResult Delete(Supplier supplier)
        {
            
            Employee.LoggedInEmployee.DeleteSupplier(supplier);
            return RedirectToAction("Index");
        }
    }
}
