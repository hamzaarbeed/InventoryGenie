using InventoryGenie.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryGenie.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<OrderRecord> OrderRecords { get; set; } = null!;
        public DbSet<SaleRecord> SaleRecords { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //to make Username unique in db
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            //to make Product Name unique in db
            modelBuilder.Entity<Product>().HasIndex(u => u.Name).IsUnique();
            // note probably needs validation at data entry

            

            modelBuilder.Entity<Role>().HasData(

                new Role
                {
                    RoleId = 1,
                    RoleName = "General Manager",
                },
                new Role
                {
                    RoleId = 2,
                    RoleName = "Warehouse Leader",
                },
                new Role
                {
                    RoleId = 3,
                    RoleName = "Associate",
                }
                );
            addDummyData(modelBuilder);
        }
        private void addDummyData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(

                new User
                {
                    Id = 1,
                    UserName = "admin",
                    RoleId = 1,
                    FirstName = "Tom",
                    LastName = "Smith",
                    Password = "password",
                }
            );

        }
    }
}
