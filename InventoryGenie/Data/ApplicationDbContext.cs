using InventoryGenie.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryGenie.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<OrderRecord> OrderRecords { get; set; } = null!;
        public DbSet<SaleRecord> SaleRecords { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //to make Username unique in db
            modelBuilder.Entity<Employee>().HasIndex(u => u.UserName).IsUnique();
            //to make Product Name unique in db
            modelBuilder.Entity<Product>().HasIndex(u => u.Name).IsUnique();
            // note probably needs validation at data entry

            addDummyData(modelBuilder);
        }
        private void addDummyData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(

                new Employee
                {
                    Id = 1,
                    UserName = "admin",
                    Role = Employee.RoleType.General_Manager,
                    FirstName = "Tom",
                    LastName = "Smith",
                    Password = "password",
                },
                new Employee
                {
                    Id = 2,
                    UserName = "wl",
                    Role = Employee.RoleType.Warehouse_Leader,
                    FirstName = "William",
                    LastName = "Leonard",
                    Password = "password",
                    IsTemporaryPassword = false,
                },
                new Employee
                {
                    Id=3,
                    UserName = "associate",
                    Role = Employee.RoleType.Associate,
                    FirstName = "Adam",
                    LastName = "Cash",
                    Password = "password",
                    IsTemporaryPassword = false,
                }
            );

            modelBuilder.Entity<Supplier>().HasData(
                    new Supplier
                    {
                        Id = 1,
                        SupplierName = "Monster",
                        
                    },
                    new Supplier
                    {
                        Id = 2,
                        SupplierName = "Hamza",
                    }

                );

            modelBuilder.Entity<Product>().HasData(
                    new Product
                    {
                        Id = 1,
                        Name = "Monster Energy Drink",
                        Category = "Energy Drinks",
                        Quantity = 400,
                        Description = "Energy Drinks",
                        ShelfPrice = 1.52,
                        WholesalePrice = 1.15,
                        MinimumLevel = 30,
                        MaximumLevel= 500,
                        SupplierID = 1
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Lays Chips",
                        Category = "Snacks",
                        Quantity = 100,
                        Description = "Lays Chips Original Taste",
                        ShelfPrice = 2.52,
                        WholesalePrice = 2.00,
                        MinimumLevel = 30,
                        MaximumLevel = 150,
                        SupplierID = 2
                    }

                );

        }
    }
}
