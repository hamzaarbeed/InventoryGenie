using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class SupplierController : Controller
    {
        private static string? SearchText;
        private static string? SortBy;
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
                SortBy = "Supplier ID";
                SearchText = null;
                return RedirectToAction("Search");
            }
        }

        [HttpPost]
        public IActionResult Search(string searchText, string sortBy)
        {
            SearchText = searchText;
            SortBy = sortBy;
            return RedirectToAction("Search");
        }

        [HttpGet]
        public IActionResult Search()
        {
            ViewBag.SortByOptions = sortByOptions;
            List<Supplier> suppliers =
                Employee.LoggedInEmployee.SearchSuppliers(SortBy, SearchText);
            return View("Index",suppliers);
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            ViewBag.Suppliers = Employee.LoggedInEmployee.GetAllSuppliers();
            Supplier supplier=Employee.LoggedInEmployee.GetSupplierByID(id);
            return View(supplier);
        }

        [HttpPost]
        
        public IActionResult ChangeProductSupplier(int productID,int newSupplierID)
        {
            Product product = Employee.LoggedInEmployee.GetProductByID(productID);
            int? currentSupplierID = product.SupplierId;
            product.SupplierId = newSupplierID;
            Employee.LoggedInEmployee.UpdateProduct(product);

            return RedirectToAction("Details",new { id= currentSupplierID });
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
                return RedirectToAction("Search");
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
            return RedirectToAction("Search");
        }

        [HttpPost]
        public IActionResult ChangeState(int supplierID)
        {
            Supplier supplier = Employee.LoggedInEmployee.GetSupplierByID(supplierID);

            //Change supplier from active to inactive or the opposite
            supplier.IsActive = supplier.IsActive ? false : true;

            // Change all products of active supplier to active or the opposite
            foreach (Product product in supplier.Products)
            {
                product.IsActive = supplier.IsActive;
            }

            Employee.LoggedInEmployee.UpdateSupplier(supplier);
            return RedirectToAction("Search");
        }
    }
}
