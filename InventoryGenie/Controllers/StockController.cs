using InventoryGenie.Data;
using InventoryGenie.Models;
using InventoryGenie.Models.AllEmployeesFunctions;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class StockController : Controller
    {
        public StockController(ApplicationDbContext ctx)
        {
            Employee.Context = ctx;
        }


        [HttpPost]
        public IActionResult Search(string searchText)
        {
            List<Product> products= Employee.LoggedInEmployee.SearchProducts(
                Associate.SortProductByType.Default, searchText,
                true,true,false,true,false,true,false,true,false,false);
            return View("Index", products);
        }

        [HttpPost]
        public IActionResult Update(int newQuantity,int productID)
        {
            Employee.LoggedInEmployee.ChangeQuantityTo(newQuantity,productID);
            return RedirectToAction("Index","Stock");
        }


        [HttpGet]
        public IActionResult Index(List<Product> products)
        {
            if (Employee.LoggedInEmployee == null)
                return RedirectToAction("Index", "Login");
            else
            {
                if (!products.Any())
                    products = Employee.LoggedInEmployee.GetAllProductsList(Associate.SortProductByType.Default);
                return View(products);
            }
        }
    }
}
