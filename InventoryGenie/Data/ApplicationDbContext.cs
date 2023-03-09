﻿using InventoryGenie.Models;
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //to make Username unique in db
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            //to make Product Name unique in db
            modelBuilder.Entity<Product>().HasIndex(u => u.Name).IsUnique();
            // note probably needs validation at data entry

            addDummyData(modelBuilder);
        }
        private void addDummyData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(

                new User
                {
                    Id = 1,
                    UserName = "admin",
                    Role = User.RoleType.General_Manager,
                    FirstName = "Tom",
                    LastName = "Smith",
                    Password = "password",
                },
                new User
                {
                    Id = 2,
                    UserName = "wl",
                    Role = User.RoleType.Warehouse_Leader,
                    FirstName = "William",
                    LastName = "Leonard",
                    Password = "password",
                    ChangePassword = false,
                },
                new User
                {
                    Id=3,
                    UserName = "associate",
                    Role = User.RoleType.Associate,
                    FirstName = "Adam",
                    LastName = "Cash",
                    Password = "password",
                    ChangePassword = false,
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
                        SellingPrice = 1.52,
                        Cost = 1.15,
                        MinimumLevel = 30,
                        MaximumLevel= 500,
                        SupplierName ="Monster Company"
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Lays Chips",
                        Category = "Snacks",
                        Quantity = 100,
                        Description = "Lays Chips Original Taste",
                        SellingPrice = 2.52,
                        Cost = 2.00,
                        MinimumLevel = 30,
                        MaximumLevel = 150,
                        SupplierName = "Lays"
                    }

                );

        }
    }
}
