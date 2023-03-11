namespace InventoryGenie.Models.AllEmployeesFunctions
{
    public class AssociateFunctions:EmployeeFunctions
    {
        public static List<Product> GetAllProductsList()
        {
            return Context.Products.ToList();
        }

        // Might change Search to Modular where you specify to search by what
        public static List<Product> Search(string searchText)
        {
            return Context.Products.Where(x => x.Name.Contains(searchText)).ToList(); ;
        }

        public static void ChangeQuantitiyTo( int newQuantity, int productID)
        {
            Context.Products.Find(productID).Quantity = newQuantity;
            Context.SaveChanges();
        }
    }
}
