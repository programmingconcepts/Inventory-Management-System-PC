using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_Management_System_PC.Models
{
    public class SupplyDetail
    {
        [Key]
        public int SupplyDetailId { get; set; }

        [Required]
        public int SupplyId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public double PurchasePrice { get; set; }

        [Required]
        public double Quantity { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        [ForeignKey("SupplyId")]
        public virtual Supply Supply { get; set; }
    }
}
