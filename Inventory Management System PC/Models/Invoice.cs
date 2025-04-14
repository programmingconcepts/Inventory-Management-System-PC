using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System_PC.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public DateTime Date { get; set; }

        public decimal ExtraCharges { get; set; }

        public decimal Discount { get; set; }

        public decimal PaidAmount { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
