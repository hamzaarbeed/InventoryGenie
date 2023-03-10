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
        public DbSet<Role> Roles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //prevent deleting all products that has certain supplier when deleting that supplier
            modelBuilder.Entity<Supplier>().HasMany(b => b.Products).WithOne(x => x.Supplier).
                OnDelete(DeleteBehavior.SetNull);

            //to make Username unique in db
            modelBuilder.Entity<Employee>().HasIndex(u => u.UserName).IsUnique();
            //to make Product Name unique in db
            modelBuilder.Entity<Product>().HasIndex(u => u.Name).IsUnique();
            // note probably needs validation at data entry

            //these must exist in db Everytime the application start
            addEssentialData(modelBuilder);

            //these are optional. For demonstration purpose
            addDummyData(modelBuilder);
        }

        private void addEssentialData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(

                new Role
                {
                    RoleID = 1,
                    RoleName = "General Manager"
                },
                new Role
                {
                    RoleID = 2,
                    RoleName = "Warehouse Leader"
                },
                new Role
                {
                    RoleID = 3,
                    RoleName = "Associate"
                }
            );
            modelBuilder.Entity<Employee>().HasData(

                new Employee
                {
                    EmployeeID = 1,
                    UserName = "admin",
                    RoleId = 1,
                    FirstName = "Tom",
                    LastName = "Smith",
                    Password = "password",
                    IsTemporaryPassword = false,
                }
            ) ;

            //when deleting a supplier products connected to that supplier will hava supplier ID = 9
            modelBuilder.Entity<Supplier>().HasData(

                new Supplier
                {
                    SupplierID = 1000,
                    SupplierName = "Unspecified",
                }
            );

        }
        private void addDummyData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(

                new Employee
                {
                    EmployeeID = 2,
                    UserName = "E1002",
                    RoleId = 2,
                    FirstName = "William",
                    LastName = "Leonard",
                    Password = "password",
                    IsTemporaryPassword = false,
                },
                new Employee
                {
                    EmployeeID =3,
                    UserName = "E1003",
                    RoleId = 3,
                    FirstName = "Adam",
                    LastName = "Cash",
                    Password = "password",
                    IsTemporaryPassword = true,
                }
            );

            modelBuilder.Entity<Supplier>().HasData(
                    new Supplier
                    {
                        SupplierID = 1,
                        SupplierName = "Monster",
                        
                    },
                    new Supplier
                    {
                        SupplierID = 2,
                        SupplierName = "Lays",
                    }

                );

            modelBuilder.Entity<Product>().HasData(
                    new Product
                    {
                        ProductID = 1,
                        Name = "Monster Energy Drink",
                        Category = "Energy Drinks",
                        Quantity = 400,
                        Description = "Energy Drinks",
                        ShelfPrice = 1.52,
                        WholesalePrice = 1.15,
                        MinimumLevel = 30,
                        MaximumLevel= 500,
                        SupplierId = 1
                    },
                    new Product
                    {
                        ProductID = 2,
                        Name = "Lays Chips",
                        Category = "Snacks",
                        Quantity = 100,
                        Description = "Lays Chips Original Taste",
                        ShelfPrice = 2.52,
                        WholesalePrice = 2.00,
                        MinimumLevel = 30,
                        MaximumLevel = 150,
                        SupplierId = 2
                    }

                );

        }
    }
}
