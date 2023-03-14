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
    }
}
