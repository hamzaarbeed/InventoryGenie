using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace InventoryGenie.Models.AllEmployeesFunctions
{
    public class Associate:Employee
    {
               

        public override List<Product> StockManagementSearchProducts(string sortBy, string searchText)
        {
            IQueryable<Product> query;
            if (searchText != null)
            {
                query = Context.Products.Where(x =>
                    x.ProductID.ToString().Contains(searchText) ||
                    x.Quantity.ToString().Contains(searchText) ||
                    x.MinimumLevel.ToString().Contains(searchText) ||
                    x.Name.Contains(searchText));
            }
            else
            {
                query = Context.Products;
            }
            switch (sortBy)
            {
                default:
                case "Product ID":
                    return query.OrderBy(x => x.ProductID).ToList();
                case "Name":
                    return query.OrderBy(x => x.Name).ToList();
                case "Quantity":
                    return query.OrderBy(x => x.Quantity).ToList();
                case "Minimum Level":
                    return query.OrderBy(x => x.MinimumLevel).ToList();
            }
        }

        public override void ChangeQuantityTo( int newQuantity, int productID)
        {
            Context.Products.Find(productID).Quantity = newQuantity;
            Context.SaveChanges();
        }

        //quantityChange can be positive(to increase Quantity) or negative (to decrease Quantity)
        public override void ChangeQuantityBy(int quantityChange, Product product)
        {
            product.Quantity += quantityChange;
            Context.SaveChanges();
        }
        public override void ChangeQuantityBy(int quantityChange, int productID)
        {
            Context.Products.Find(productID).Quantity += quantityChange;
            Context.SaveChanges();
        }
        //quantityExchanged can be positive(sold) or be negative(returned)
        public override void CheckOut(int quantityExchanged, int productID)
        {
            Product product = Context.Products.Find(productID);
            SaleRecord SaleRecord = new SaleRecord()
            {
                ProductId = productID,
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
