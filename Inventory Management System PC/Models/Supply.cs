using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_Management_System_PC.Models
{
    public class Supply
    {
        [Key]
        public int SupplyId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<SupplyDetail> SupplyDetails { get; set; }
    }
}
