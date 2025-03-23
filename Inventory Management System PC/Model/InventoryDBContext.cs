using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System_PC.Model
{
    public class InventoryDBContext : DbContext
    {
        public InventoryDBContext() : base("name=ConStr")
        {
        }
        public DbSet<Item> Items { get; set; }
    }
}
