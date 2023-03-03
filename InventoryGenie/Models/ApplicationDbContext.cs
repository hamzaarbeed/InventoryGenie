using Microsoft.EntityFrameworkCore;
using InventoryGenie.Data;

namespace InventoryGenie.Models
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
        }
    }
}
