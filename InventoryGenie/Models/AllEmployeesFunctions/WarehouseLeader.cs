using Microsoft.EntityFrameworkCore;

namespace InventoryGenie.Models.AllEmployeesFunctions
{
    public class WarehouseLeader:Associate
    {
        public override List<Product> GetStockOut()
        {
            return Context.Products.Where(x=>x.Quantity ==0 && x.IsActive).ToList();
        }
        public override List<Product> GetLowStock()
        {
            return Context.Products.Where(x => 
                x.Quantity > 0 && x.Quantity <= x.MinimumLevel && x.IsActive).ToList();
        }

        public override List<Supplier> SearchSuppliers(string sortBy, string searchText)
        {
            IQueryable<Supplier> query;
            if (searchText != null && searchText != string.Empty)
            {
                query = Context.Suppliers.Include(x=>x.Products).Where(x =>
                    x.SupplierID.ToString().Contains(searchText) ||
                    x.SupplierName.Contains(searchText) 
                );
            }
            else
            {
                query = Context.Suppliers.Include(x => x.Products);
            }

            switch (sortBy)
            {
                default:
                case "Supplier ID":
                    return query.OrderBy(x => x.SupplierID).ToList();
                case "Name":
                    return query.OrderBy(x => x.SupplierName).ToList();
            }
        }

        public override Supplier GetSupplierByID(int supplierID) {
            return Context.Suppliers.Include(x=>x.Products).FirstOrDefault(x=>x.SupplierID == supplierID);
        }

        public override void CreateSupplier(Supplier supplierID)
        {
            Context.Suppliers.Add(supplierID);
            Context.SaveChanges();
        }

        public override void UpdateSupplier(Supplier supplierID)
        {
            Context.Suppliers.Update(supplierID);
            Context.SaveChanges();
        }

        public override void DeleteSupplier(Supplier supplier)
        {
            Context.Suppliers.Remove(supplier);
            Context.SaveChanges();
        }

        public override List<Product> ProductManagementSearchProducts(string sortBy, string searchText)
        {
            IQueryable<Product> query;
            if (searchText != null)
            {
                query = Context.Products.Include(x=>x.Category).Where(x =>
                    x.ProductID.ToString().Contains(searchText) ||
                    x.Name.Contains(searchText) ||
                    x.Category.Name.Contains(searchText) ||
                    x.Description.Contains(searchText));
            }
            else
            {
                query = Context.Products.Include(x=>x.Category);
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
                case "Description":
                    return query.OrderBy(x => x.Description).ToList();
            }
        }

        public override List<Category> GetAllCategories()
        {
            return Context.Categories.OrderBy(x => x.Name).ToList();
        }

        public override List<Supplier> GetAllSuppliers()
        {
            return Context.Suppliers.OrderBy(x => x.SupplierName).ToList();
        }

        public override void CreateProduct(Product productID)
        {
            Context.Products.Add(productID);
            Context.SaveChanges();
        }

        public override void UpdateProduct(Product productID)
        {
            Context.Products.Update(productID);
            Context.SaveChanges();
        }


        public override void DeleteProduct(Product product)
        {
            Context.Products.Remove(product);
            Context.SaveChanges();
        }
    }
}
