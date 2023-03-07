using InventoryGenie.Data;
using InventoryGenie.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryGenie.Controllers
{
    public class StockController : Controller
    {
        private ApplicationDbContext context { get; set; }
        public StockController(ApplicationDbContext ctx)
        {
            context = ctx;
        }


        [HttpPost]
        public IActionResult Search(string searchText)
        {
            List<Product> products= context.Products.Where(x=>x.Name.Contains(searchText)).ToList();
            return View("Index", products);
        }

        [HttpPost]
        public IActionResult Update(int newQuantity,int productID)
        {
            Product product = context.Products.Find(productID);
            product.Quantity = newQuantity;
            context.SaveChanges();
            return RedirectToAction("Index","Stock");
        }


        [HttpGet]
        public IActionResult Index(List<Product> products)
        {
            if (TempData.Peek("UserID") == null)
                return RedirectToAction("Index", "Login");
            else
            {
                if (!products.Any())
                    products = context.Products.ToList();
                return View(products);
            }
        }
    }
}
