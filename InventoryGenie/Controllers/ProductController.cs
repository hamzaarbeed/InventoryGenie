using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;


namespace InventoryGenie.Controllers
{
    public class ProductController : Controller
    {
        //holds SearchText and SortBy 
        private static string? SearchText;
        private static string? SortBy;

        //Gets all Suppliers and all Categories to save many trips to database
        private static List<Supplier> Suppliers = new();
        private static List<Category> Categories = new();

        //sortby options
        readonly string[] sortByOptions =
        {
            "Product ID",
            "Name",
            "Description",
            "Category",
            
        };

        //give DbContext to Employee 
        public ProductController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
        }

        //first function to be called in this controller
        [HttpGet]
        public IActionResult Index()
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            Categories = Employee.LoggedInEmployee.GetAllCategories();
            Suppliers = Employee.LoggedInEmployee.GetAllSuppliers();

            //The default settings for sortby and searchtext
            SortBy = "Product ID";
            SearchText = null;

            //after getting default settings set up redirect to HttpGet Search to show all products in Index view 
            return RedirectToAction("Search");
            
        }

        // when pressing search button
        //it set the static fields sortby and searchtext to values passed in the parameter
        // then redirect to HttpGet Search to show the products accordingly
        [HttpPost]
        public IActionResult Search(string searchText, string sortBy)
        {
            SortBy = sortBy;
            SearchText= searchText;
            return RedirectToAction("Search");
        }

        //This is the function that will show the products according to SortBy and SearchText
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
            ViewBag.SortBy = SortBy;
            ViewBag.SearchText = SearchText;
            ViewBag.QuantityNotReceivedCount = QuantityNotReceivedCount;
            return View("Index",products);
        }


        //this shows details of a product
        [HttpGet]
        public IActionResult Details(int id)
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            Product product = Employee.LoggedInEmployee.GetProductByID(id);
            return View(product);
        }

        //this prepare the ViewBag for HttpGet Search
        private void PrepareViewBagFor(string actionName)
        {
            ViewBag.Action = actionName;
            ViewBag.Categories = Categories;
            ViewBag.Suppliers = Suppliers;
        }

        //to add new product
        [HttpGet]
        public IActionResult Add()
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            PrepareViewBagFor("Add");
            return View("Edit", new Product());
        }


        //to add new product
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

        //to edit a product
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            Product product = Employee.LoggedInEmployee.GetProductByID(id);
            PrepareViewBagFor("Edit");
            return View("Edit", product);
        }

        //to edit a product
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


        //to delete a product
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsAuthenticatedAndAuthorized())
                return RedirectToAction("Index", "Home");

            Product product = Employee.LoggedInEmployee.GetProductByID(id);
            if (product.Quantity !=0 || Employee.LoggedInEmployee.GetQuantityNotReceivedForProduct(product.Name) != 0)
                return RedirectToAction("Search");
            return View("Delete", product);
        }

        //to delete a product
        [HttpPost]
        public IActionResult Delete(Product product)
        {
            Employee.LoggedInEmployee.DeleteProduct(product);
            return RedirectToAction("Search");
        }

        //to change state of product from active to inactive.
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
