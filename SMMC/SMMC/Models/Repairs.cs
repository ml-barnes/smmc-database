using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Repairs
    {
        public int InstrumentInventoryId { get; set; }
        public int TechnicianId { get; set; }
        public DateTime RepairStart { get; set; }
        public string Notes { get; set; }
        public DateTime? RepairEnd { get; set; }

        public InstrumentInventory InstrumentInventory { get; set; }
        public Technician Technician { get; set; }
    }
}
