using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;


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
            Categories = Employee.LoggedInEmployee.GetAllCategories();
            Suppliers = Employee.LoggedInEmployee.GetAllSuppliers();
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
                SortBy = "Product ID";
                SearchText = null;
                return RedirectToAction("Search");
            }
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
                return RedirectToAction("Search");
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
