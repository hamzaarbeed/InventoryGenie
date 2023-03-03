using Microsoft.EntityFrameworkCore;
using InventoryGenie.Data;

namespace InventoryGenie.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        DbSet<User> Users { get; set; } = null!;
        DbSet<OrderRecord> orderRecords { get; set; } = null!;
        DbSet<SaleRecord> saleRecords { get; set; } = null!;
        DbSet<Product> products { get; set; } = null!;
        DbSet<Role> roles { get; set; } = null!;

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
