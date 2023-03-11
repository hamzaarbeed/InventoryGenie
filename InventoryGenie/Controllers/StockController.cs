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
            List<Product> products= AssociateFunctions.SearchProducts(
                AssociateFunctions.SortProductByType.Default, searchText,
                true,true,false,true,false,true,false,true,false,false);
            return View("Index", products);
        }

        [HttpPost]
        public IActionResult Update(int newQuantity,int productID)
        {
            AssociateFunctions.ChangeQuantityTo(newQuantity,productID);
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
                    products = AssociateFunctions.GetAllProductsList(AssociateFunctions.SortProductByType.Default);
                return View(products);
            }
        }
    }
}
