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
                List<Product> products =
                    Employee.LoggedInEmployee.ProductManagementSearchProducts(defaultSortBy, null);
                return View(products);
            }
        }

        [HttpPost]
        public IActionResult Index(string searchText, string sortBy)
        {
            ViewBag.SortByOptions = sortByOptions;
            List<Product> products =
                Employee.LoggedInEmployee.ProductManagementSearchProducts(sortBy, searchText);
            return View(products);
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            Product product = Employee.LoggedInEmployee.GetProductByID(id);
            return View(product);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            ViewBag.Categories = Employee.LoggedInEmployee.GetAllCategories();
            ViewBag.Suppliers = Employee.LoggedInEmployee.GetAllSuppliers();
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
                    TempData["Msg"] = "This product name already in use";
                    return RedirectToAction("Add");
                }
                //fully load product with category and Supplier to be able to show them in "Details" view
                product =Employee.LoggedInEmployee.GetProductByID(product.ProductID);
                return View("Details",product);
                
            }
            return RedirectToAction("Add");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Product product = Employee.LoggedInEmployee.GetProductByID(id);
            ViewBag.Action = "Edit";
            ViewBag.Categories = Employee.LoggedInEmployee.GetAllCategories();
            ViewBag.Suppliers = Employee.LoggedInEmployee.GetAllSuppliers();
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

            ViewBag.Action = "Edit";
            ViewBag.Categories = Employee.LoggedInEmployee.GetAllCategories();
            ViewBag.Suppliers = Employee.LoggedInEmployee.GetAllSuppliers();
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
