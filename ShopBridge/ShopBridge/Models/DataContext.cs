using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options)
             : base(options)
        {

        }

        public DbSet<Inventory> inventories { get; set; }
    }
}
