using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMMC.ViewModels
{
    public class AddInstrumentViewModel
    {
        public int InstrumentInventoryId { get; set; }
        public int InstrumentId { get; set; }        
        public string Manufacturer { get; set; }
        public int Cost { get; set; }

        public int Quantity { get; set; }


    }
}
