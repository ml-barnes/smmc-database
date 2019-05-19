using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class SheetMusicInventory
    {
        public int SheetMusicInventoryId { get; set; }
        public int SheetMusicId { get; set; }
        public DateTime? PurchaseDate { get; set; }

        public SheetMusic SheetMusic { get; set; }
    }
}
