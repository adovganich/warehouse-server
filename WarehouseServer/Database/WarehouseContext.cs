﻿using Microsoft.EntityFrameworkCore;
using WarehouseServer.Model;

namespace WarehouseServer.Database
{
    public class WarehouseContext : DbContext
    {

        public WarehouseContext()
        {
        }

        public WarehouseContext(DbContextOptions<WarehouseContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=warehouse.db");
            }
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
