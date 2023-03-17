namespace InventoryGenie.Models
{
    public static class DbQueriesHolder
    {
        public static Dictionary<int, int> Cart = new();
        public static List<Product> Products = new();
        public static List<Employee> Employees = new();
        public static List<Supplier> Suppliers = new();
        public static List<OrderRecord> OrderRecords = new();
        public static List<SaleRecord> SaleRecords = new();
        public static List<Category> Categories = new();
        public static List<Role> Roles = new();

    }
}
