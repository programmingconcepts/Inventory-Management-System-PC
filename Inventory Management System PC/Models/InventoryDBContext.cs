using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System_PC.Models
{
    public class InventoryDBContext : DbContext
    {
        public InventoryDBContext() : base("ConStr")
        {
        }
        public DbSet<Item> Items { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Supply> Supplies { get; set; }

        public DbSet<SupplyDetail> SupplyDetails { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>()
                .HasRequired(s => s.Item)
                .WithMany()
                .HasForeignKey(i => i.ItemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stock>()
                .HasRequired(s => s.SupplyDetail)
                .WithMany()
                .HasForeignKey(sd => sd.SupplyDetailId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SupplyDetail>()
                .HasRequired(s => s.Item)
                .WithMany()
                .HasForeignKey(i => i.ItemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SupplyDetail>()
                .HasRequired(sd => sd.Supply)
                .WithMany()
                .HasForeignKey(s => s.SupplyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supply>()
                .HasRequired(s => s.Supplier)
                .WithMany()
                .HasForeignKey(sp => sp.SupplierId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
