using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_System_PC.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [MaxLength(15)]
        public string PhoneNumber { get; set; }


        [MaxLength(100)]
        public string Address { get; set; }
    }
}
