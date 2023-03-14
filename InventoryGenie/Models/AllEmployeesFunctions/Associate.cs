using Microsoft.EntityFrameworkCore;

namespace InventoryGenie.Models.AllEmployeesFunctions
{
    public class Associate:Employee
    {
        

        protected List<Product> SortProductByFunction(Product.SortProductByType sortBy, IQueryable<Product> Query)
        {
            if (sortBy == Product.SortProductByType.Default) 
                return Query.ToList();

            return Query.OrderBy(x => x.GetType().GetProperty(sortBy.ToString().Replace("_", ""))).ToList();
        }
        public override List<Product> GetAllProductsList(Product.SortProductByType sortBy)
        {
            return SortProductByFunction(sortBy,Context.Products);
        }

        // Might change Search to Modular where you specify to search by what
        public override List<Product> SearchProducts(Product.SortProductByType sortBy, string searchText, bool byProductID, bool byName, bool byCategory,
            bool byDescription, bool bySupplierName, bool byQuantity, bool byMaximumLevel, bool byMinimumLevel,
            bool byWholesalePrice, bool byShelfPrice)
        {
  
            IQueryable<Product> Query = Context.Products.Include(x => x.Supplier).Where(x =>
                byProductID? x.ProductID.ToString().Contains(searchText):false ||
                byQuantity ? x.Quantity.ToString().Contains(searchText) : false ||
                byMaximumLevel ? x.MaximumLevel.ToString().Contains(searchText) : false ||
                byMinimumLevel ? x.MinimumLevel.ToString().Contains(searchText) : false ||
                byWholesalePrice?x.WholesalePrice.ToString().Contains(searchText):false ||
                byShelfPrice ? x.ShelfPrice.ToString().Contains(searchText) : false ||
                byName ? x.Name.Contains(searchText) : false ||
                byCategory ? x.Category.Contains(searchText) : false ||
                byDescription ? x.Description.Contains(searchText) : false ||
                bySupplierName ? x.Supplier.SupplierName.Contains(searchText) : false
            );


            return SortProductByFunction(sortBy, Query);

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
