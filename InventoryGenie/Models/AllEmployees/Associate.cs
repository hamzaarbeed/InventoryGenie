using InventoryGenie.Controllers;
using Microsoft.EntityFrameworkCore;

namespace InventoryGenie.Models.AllEmployees
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

        public override List<Product> SalesManagementSearchProducts(string sortBy, string searchText)
        {
            IQueryable<Product> query;
            if (searchText != null)
            {
                query = Context.Products.Include(x => x.Supplier).Include(x => x.Category).Where(x =>
                    x.ProductID.ToString().Contains(searchText) ||
                    x.Name.Contains(searchText) ||
                    x.Description.Contains(searchText) ||
                    x.Supplier.SupplierName.Contains(searchText) ||
                    x.Category.Name.Contains(searchText));
            }
            else
            {
                query = Context.Products.Include(x => x.Supplier).Include(x => x.Category);
            }
            switch (sortBy)
            {
                default:
                case "Product ID":
                    return query.OrderBy(x => x.ProductID).ToList();
                case "Name":
                    return query.OrderBy(x => x.Name).ToList();
                case "Category":
                    return query.OrderBy(x => x.Category.Name).ToList();
                case "Supplier":
                    return query.OrderBy(x => x.Supplier.SupplierName).ToList();
                case "Shelf Price":
                    return query.OrderBy(x => x.ShelfPrice).ToList();
            }
        }

        public override void ChangeQuantityTo( int newQuantity, int productID)
        {
            Context.Products.Find(productID).Quantity = newQuantity;
            Context.SaveChanges();
        }

        
        //quantityExchanged can be positive(sold) or be negative(returned)
        public override void ProcessTransaction(List<Product> productsInCart,Dictionary<int,int> cart)
        {
            for (int i =0; i < cart.Count(); i++) {
                Product product = productsInCart[i];
                int quantityInCart = cart.GetValueOrDefault(product.ProductID);

                product.Quantity -= quantityInCart;
                Context.Products.Update(product);
                SaleRecord SaleRecord = new SaleRecord()
                {
                    ProductName = product.Name,
                    SupplierName = product.Supplier.SupplierName,
                    QuantityExchanged = quantityInCart,
                    ShelfPrice = Math.Round(product.ShelfPrice * quantityInCart,2),
                    WholesalePrice = Math.Round(product.WholesalePrice * quantityInCart,2),
                    CreatedOn = DateTime.Now,
                    CategoryName = product.Category.Name,
                };
                Context.SaleRecords.Add(SaleRecord);
            }
            Context.SaveChanges();
        }

        public override Product? GetProductByID(int productID)
        {
            return Context.Products.Include(x => x.Category).Include(x => x.Supplier).FirstOrDefault(x => x.ProductID == productID);
        }
    }
}
