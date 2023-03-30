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
            if (!IsAuthenticatedAndAuthorized())
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
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            ViewBag.SortByOptions = sortByOptions;
            List<Product> products =
                Employee.LoggedInEmployee.ProductManagementSearchProducts(SortBy, SearchText);
            int[] QuantityNotReceivedCount = new int[products.Count];
            for (int i =0; i< products.Count ; i++ )  
            {
                QuantityNotReceivedCount[i] = Employee.LoggedInEmployee.GetQuantityNotReceivedForProduct(products[i].Name);
            }

            ViewBag.QuantityNotReceivedCount = QuantityNotReceivedCount;
            return View("Index",products);
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            if (!IsAuthenticatedAndAuthorized())
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
            if (!IsAuthenticatedAndAuthorized())
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
            if (!IsAuthenticatedAndAuthorized())
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
            if (!IsAuthenticatedAndAuthorized())
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

        // if the LoggedInEmployee is not null and role is GM (1) or WL(2) then return true.
        // if it's not true then user will be redirected to Home.
        // Home will redirect to login if there is no logged in Employee. 
        private static bool IsAuthenticatedAndAuthorized()
        {
            return Employee.LoggedInEmployee != null && (Employee.LoggedInEmployee.RoleId == 1 || Employee.LoggedInEmployee.RoleId == 2);
        }
    }
}
