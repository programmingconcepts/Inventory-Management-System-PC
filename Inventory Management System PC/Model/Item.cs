﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Management_System_PC.Model
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Details { get; set; }

        public double Price { get; set; }

        // Units, Kg, Liters, etc.
        public string MeasuringUnit { get; set; }

        public double ReOrderLevel { get; set; } = 10;
    }
}
