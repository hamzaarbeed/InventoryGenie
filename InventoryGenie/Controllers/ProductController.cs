using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;


namespace InventoryGenie.Controllers
{
    public class ProductController : Controller
    {
        readonly string[] sortByOptions =
        {
            "Product ID",
            "Name",
            "Description",
            "Category",
            
        };
        public ProductController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
            ApplicationDbContext.QCategories = Employee.LoggedInEmployee.GetAllCategories();
            ApplicationDbContext.QSuppliers = Employee.LoggedInEmployee.GetAllSuppliers();
        }


        [HttpGet]
        public IActionResult Index()
        {
            if (Employee.LoggedInEmployee.RoleId != 1 && Employee.LoggedInEmployee.RoleId != 2)
            {
                Employee.Logout();
                return RedirectToAction("Index", "Login");
            }else
            {
                ViewBag.SortByOptions = sortByOptions;
                string defaultSortBy = "Product ID";
                ApplicationDbContext.QProducts =
                    Employee.LoggedInEmployee.ProductManagementSearchProducts(defaultSortBy, null);
                return View(ApplicationDbContext.QSuppliers);
            }
        }

        [HttpPost]
        public IActionResult Index(string searchText, string sortBy)
        {
            ViewBag.SortByOptions = sortByOptions;
            ApplicationDbContext.QProducts =
                Employee.LoggedInEmployee.ProductManagementSearchProducts(sortBy, searchText);
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
            Product product = Employee.LoggedInEmployee.GetProductByID(id);
            return View(product);
        }

        private void PrepareViewBagFor(string actionName)
        {
            ViewBag.Action = actionName;
            ViewBag.Categories = ApplicationDbContext.QCategories;
            ViewBag.Suppliers = ApplicationDbContext.QSuppliers;
        }

        [HttpGet]
        public IActionResult Add()
        {
            PrepareViewBagFor("Add");
            return View("Edit", new Product());
        }

        

        [HttpPost]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Employee.LoggedInEmployee.CreateProduct(product);
                }catch (Exception)
                {
                    ViewBag.Msg = "This product name already in use";
                    PrepareViewBagFor("Add");
                    return View("Edit", product);
                }
                //fully load product with category and Supplier to be able to show them in "Details" view
                product =Employee.LoggedInEmployee.GetProductByID(product.ProductID);
                return View("Details",product);
                
            }
            PrepareViewBagFor("Add");
            return View("Edit",product);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Product product = Employee.LoggedInEmployee.GetProductByID(id);
            PrepareViewBagFor("Edit");
            return View("Edit", product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                Employee.LoggedInEmployee.UpdateProduct(product);
                return RedirectToAction("Index");
            }

            PrepareViewBagFor("Edit");
            return View("Edit", product);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {

            Product product = Employee.LoggedInEmployee.GetProductByID(id);
            return View("Delete", product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {

            Employee.LoggedInEmployee.DeleteProduct(product);
            return RedirectToAction("Index");
        }
    }
}
