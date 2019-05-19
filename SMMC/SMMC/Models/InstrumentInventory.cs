using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class InstrumentInventory
    {
        public InstrumentInventory()
        {
            Loan = new HashSet<Loan>();
            Repairs = new HashSet<Repairs>();
        }

        public int InstrumentInventoryId { get; set; }
        public int InstrumentId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Manufacturer { get; set; }
        public int Cost { get; set; }

        public Instrument Instrument { get; set; }
        public ICollection<Loan> Loan { get; set; }
        public ICollection<Repairs> Repairs { get; set; }
    }
}
