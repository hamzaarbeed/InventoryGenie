using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
            if (searchText != null)
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
        public override int GetQuantityNotReceivedForProduct(string productName)
        {
            int total = 0;
            List<OrderRecord> orderRecords= Context.OrderRecords.Where(x=>x.ProductName== productName && x.IsReceived == false).ToList();
            foreach (OrderRecord orderRecord in orderRecords)
            {
                total += orderRecord.QuantityOrdered;
            }
            return total;
        }
        public override List<OrderRecord> SearchOrderRecords(string sortBy,string searchText)
        {
            IQueryable<OrderRecord> query;
            if (searchText != null)
            {
                query = Context.OrderRecords.Where(x =>
                    x.ProductName.Contains(searchText) ||
                    x.CategoryName.Contains(searchText) ||
                    x.SupplierName.Contains(searchText) ||
                    x.OrderedOn.ToString().Contains(searchText) ||
                    x.QuantityOrdered.ToString().Contains(searchText));
            }
            else
            {
                query = Context.OrderRecords;
            }
            
            switch (sortBy)
            {
                default:
                case "Date & Time":
                    return query.OrderByDescending(x => x.OrderedOn).ToList();
                case "Product":
                    return query.OrderBy(x => x.ProductName).ToList();
                case "Category":
                    return query.OrderBy(x => x.CategoryName).ToList();
                case "Supplier":
                    return query.OrderBy(x => x.SupplierName).ToList();
            }
        }

        public override void ReceiveOrder(int orderRecordID)
        {
            OrderRecord orderRecord = Context.OrderRecords.Find(orderRecordID);
            if (orderRecord.IsReceived)
                return;
            orderRecord.IsReceived= true;
            Product product = Context.Products.FirstOrDefault(x => x.Name == orderRecord.ProductName);
            product.Quantity += orderRecord.QuantityOrdered;
            Context.SaveChanges();
        }

        public override OrderRecord GetOrderRecordByID(int orderRecordID)
        {
            return Context.OrderRecords.Find(orderRecordID);
        }

        public override List<Product> OrderManagementSearchProducts(string sortBy, string searchText , bool showOnlyBelowMinimumLevel)
        {

            IQueryable<Product> query = Context.Products.Where(x => x.IsActive == true);
            query= showOnlyBelowMinimumLevel? query.Where(x=>x.Quantity <= x.MinimumLevel): query;

            if (searchText != null)
            {
                query = query.Where(x =>
                    x.ProductID.ToString().Contains(searchText) ||
                    x.Name.Contains(searchText));
            }
            switch (sortBy)
            {
                default:
                case "Product ID":
                    return query.OrderBy(x => x.ProductID).ToList();
                case "Product Name":
                    return query.OrderBy(x => x.Name).ToList();
            }
        }

        public override void PlaceOrder(int productID,int orderQuantity)
        {
            Product product = GetProductByID(productID);
            Context.OrderRecords.Add(new OrderRecord
            {
                OrderedOn = DateTime.Now,
                QuantityOrdered = orderQuantity,
                ProductName = product.Name,
                CategoryName = product.Category.Name,
                SupplierName = product.Supplier.SupplierName,
                WholesalePrice = Math.Round(product.WholesalePrice * orderQuantity, 2)
            });
            Context.SaveChanges();
        }

        public override List<SaleRecord> GenerateSalesReport(string from,string to)
        {
            DateTime fromDateTime = DateOnly.Parse(from).ToDateTime(TimeOnly.Parse("00:00:00"));
            DateTime toDateTime = DateOnly.Parse(to).ToDateTime(TimeOnly.Parse("23:59:59"));

            IQueryable<SaleRecord> query = Context.SaleRecords.Where(x => x.CreatedOn >= fromDateTime && x.CreatedOn <= toDateTime);
            query = query.GroupBy(x => x.ProductName).Select(g => new SaleRecord
            {
                ProductName = g.First().ProductName,
                CategoryName = g.First().CategoryName,
                SupplierName = g.First().SupplierName,
                ShelfPrice = g.Sum(x=>x.ShelfPrice),
                WholesalePrice = g.Sum(x=>x.WholesalePrice),
                QuantityExchanged = g.Sum(x=>x.QuantityExchanged),
            }) ;
            return query.ToList();
        }

        public override List<OrderRecord> GenerateOrdersReport(string from, string to)
        {
            DateTime fromDateTime = DateOnly.Parse(from).ToDateTime(TimeOnly.Parse("00:00:00"));
            DateTime toDateTime = DateOnly.Parse(to).ToDateTime(TimeOnly.Parse("23:59:59"));

            IQueryable<OrderRecord> query = Context.OrderRecords.Where(x => x.OrderedOn >= fromDateTime && x.OrderedOn <= toDateTime);
            return query.ToList();
        }
    }
}
