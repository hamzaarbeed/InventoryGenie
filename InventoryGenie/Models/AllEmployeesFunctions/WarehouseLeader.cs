using Microsoft.EntityFrameworkCore;

namespace InventoryGenie.Models.AllEmployeesFunctions
{
    public class WarehouseLeader:Associate
    {
        public override int GetStockOutCount()
        {
            return Context.Products.Where(x=>x.Quantity ==0).Count();
        }
        public override int GetLowStockCount()
        {
            return Context.Products.Where(x => 
                x.Quantity > 0 && x.Quantity <= x.MinimumLevel).Count();
        }

        public override List<Supplier> SearchSuppliers(string sortBy, string searchText)
        {
            IQueryable<Supplier> query;
            if (searchText != null)
            {
                query = Context.Suppliers.Where(x =>
                    x.SupplierID.ToString().Contains(searchText) ||
                    x.SupplierName.Contains(searchText) 
                );
            }
            else
            {
                query = Context.Suppliers;
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
            return Context.Suppliers.Find(supplierID);
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
    }
}
