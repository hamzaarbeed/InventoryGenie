using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;


namespace InventoryGenie.Controllers
{
    public class ProductController : Controller
    {
        private static string? SearchText;
        private static string? SortBy;
        private static List<Supplier> Suppliers = new();
        private static List<Category> Categories = new();

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
            // if the role is not GM (1) and not WL (2) then redirect to Home.
            // Home will redirect to login if there is no logged in Employee. 
            if (Employee.LoggedInEmployee == null || Employee.LoggedInEmployee.RoleId != 1 && Employee.LoggedInEmployee.RoleId != 2)
                return RedirectToAction("Index", "Home");
            Categories = Employee.LoggedInEmployee.GetAllCategories();
            Suppliers = Employee.LoggedInEmployee.GetAllSuppliers();

            SortBy = "Product ID";
            SearchText = null;
            return RedirectToAction("Search");
            
        }

        [HttpPost]
        public IActionResult Search(string searchText, string sortBy)
        {
            SortBy = sortBy;
            SearchText= searchText;
            return RedirectToAction("Search");
        }

        [HttpGet]
        public IActionResult Search()
        {
            // if the role is not GM (1) and not WL (2) then redirect to Home.
            // Home will redirect to login if there is no logged in Employee. 
            if (Employee.LoggedInEmployee == null || Employee.LoggedInEmployee.RoleId != 1 && Employee.LoggedInEmployee.RoleId != 2)
                return RedirectToAction("Index", "Home");

            ViewBag.SortByOptions = sortByOptions;
            List<int> QuantityNotReceivedCount = new List<int>();
            List<Product> products =
                Employee.LoggedInEmployee.ProductManagementSearchProducts(SortBy, SearchText);
            foreach (Product product in products)
            {
                QuantityNotReceivedCount.Add(Employee.LoggedInEmployee.GetQuantityNotReceivedForProduct(product.Name));
            }
            ViewBag.OrdersNotReceivedCount = QuantityNotReceivedCount;
            return View("Index",products);
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            // if the role is not GM (1) and not WL (2) then redirect to Home.
            // Home will redirect to login if there is no logged in Employee. 
            if (Employee.LoggedInEmployee == null || Employee.LoggedInEmployee.RoleId != 1 && Employee.LoggedInEmployee.RoleId != 2)
                return RedirectToAction("Index", "Home");

            Product product = Employee.LoggedInEmployee.GetProductByID(id);
            return View(product);
        }

        private void PrepareViewBagFor(string actionName)
        {
            ViewBag.Action = actionName;
            ViewBag.Categories = Categories;
            ViewBag.Suppliers = Suppliers;
        }

        [HttpGet]
        public IActionResult Add()
        {
            // if the role is not GM (1) and not WL (2) then redirect to Home.
            // Home will redirect to login if there is no logged in Employee. 
            if (Employee.LoggedInEmployee == null || Employee.LoggedInEmployee.RoleId != 1 && Employee.LoggedInEmployee.RoleId != 2)
                return RedirectToAction("Index", "Home");

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
            // if the role is not GM (1) and not WL (2) then redirect to Home.
            // Home will redirect to login if there is no logged in Employee. 
            if (Employee.LoggedInEmployee == null || Employee.LoggedInEmployee.RoleId != 1 && Employee.LoggedInEmployee.RoleId != 2)
                return RedirectToAction("Index", "Home");

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
                return RedirectToAction("Search");
            }

            PrepareViewBagFor("Edit");
            return View("Edit", product);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // if the role is not GM (1) and not WL (2) then redirect to Home.
            // Home will redirect to login if there is no logged in Employee. 
            if (Employee.LoggedInEmployee == null || Employee.LoggedInEmployee.RoleId != 1 && Employee.LoggedInEmployee.RoleId != 2)
                return RedirectToAction("Index", "Home");

            Product product = Employee.LoggedInEmployee.GetProductByID(id);
            return View("Delete", product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            Employee.LoggedInEmployee.DeleteProduct(product);
            return RedirectToAction("Search");
        }

        [HttpPost]
        public IActionResult ChangeState(int productID)
        {
            Product product = Employee.LoggedInEmployee.GetProductByID(productID);
            product.IsActive = product.IsActive ? false : true;
            Employee.LoggedInEmployee.UpdateProduct(product);
            return RedirectToAction("Search");
        }
    }
}
