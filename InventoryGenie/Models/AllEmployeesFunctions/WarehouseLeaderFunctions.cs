namespace InventoryGenie.Models.AllEmployeesFunctions
{
    public class WarehouseLeaderFunctions:AssociateFunctions
    {
        public static int getStockOutCount()
        {
            return Context.Products.Where(x=>x.Quantity ==0).Count();
        }
        public static int getLowStockCount()
        {
            return Context.Products.Where(x => x.Quantity > 0 && x.Quantity <= x.MinimumLevel).Count();
        }
    }
}
