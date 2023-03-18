﻿using Microsoft.EntityFrameworkCore;

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
        public override void ProcessTransaction()
        {
            foreach (var cartItem in DbQueriesHolder.Cart) {
                Product product = GetProductByID(cartItem.Key);
                product.Quantity -= cartItem.Value;
                SaleRecord SaleRecord = new SaleRecord()
                {
                    ProductName = product.Name,
                    SupplierName = product.Supplier.SupplierName,
                    QuantityExchanged = cartItem.Value,
                    ShelfPrice = product.ShelfPrice * cartItem.Value,
                    WholesalePrice = product.WholesalePrice * cartItem.Value,
                    CreatedOn = DateTime.Now,
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