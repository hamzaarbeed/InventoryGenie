using Microsoft.EntityFrameworkCore;

namespace InventoryGenie.Models.AllEmployeesFunctions
{
    public class AssociateFunctions:EmployeeFunctions
    {
        public static List<Product> GetAllProductsList()
        {
            return Context.Products.ToList();
        }

        // Might change Search to Modular where you specify to search by what
        public static List<Product> SearchProducts(string searchText, bool byID, bool byName, bool byCategory,
            bool byDescription, bool bySupplierName, bool byQuantity, bool byMaximumLevel, bool byMinimumLevel,
            bool byWholesalePrice, bool byShelfPrice)
        {

            return Context.Products.Include(x=>x.Supplier).Where(x =>
                x.Id.ToString().Contains(byID ? searchText : null) ||
                x.Quantity.ToString().Contains(byQuantity ? searchText : null) ||
                x.MaximumLevel.ToString().Contains(byMaximumLevel ? searchText : null) ||
                x.MinimumLevel.ToString().Contains(byMinimumLevel ? searchText : null) ||
                x.WholesalePrice.ToString().Contains(byWholesalePrice ? searchText : null) ||
                x.ShelfPrice.ToString().Contains(byShelfPrice ? searchText : null) ||
                x.Name.Contains(byName ? searchText : null) ||
                x.Category.Contains(byCategory ? searchText:null) ||
                x.Description.Contains(byDescription?searchText:null) ||
                x.Supplier.SupplierName.Contains(bySupplierName?searchText:null)
           ).ToList(); ;
        }

        public static void ChangeQuantityTo( int newQuantity, int productID)
        {
            Context.Products.Find(productID).Quantity = newQuantity;
            Context.SaveChanges();
        }

        //quantityChange can be positive(to increase Quantity) or negative (to decrease Quantity)
        public static void ChangeQuantityBy(int quantityChange, Product product)
        {
            product.Quantity += quantityChange;
            Context.SaveChanges();
        }
        public static void ChangeQuantityBy(int quantityChange, int productID)
        {
            Context.Products.Find(productID).Quantity += quantityChange;
            Context.SaveChanges();
        }
        //quantityExchanged can be positive(sold) or be negative(returned)
        public static void CheckOut(int quantityExchanged, int productID)
        {
            Product product = Context.Products.Find(productID);
            SaleRecord SaleRecord = new SaleRecord()
            {
                ProductID = productID,
                QuantityExchanged = quantityExchanged,
                ShelfPrice = product.ShelfPrice * quantityExchanged,
                WholesalePrice = product.WholesalePrice * quantityExchanged,
                CreatedOn = DateTime.Now,
            };

            //changed quantityExchanged to negative to decrease Quantity
            ChangeQuantityBy(-quantityExchanged, product);
        }
    }
}
