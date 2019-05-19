using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class SheetMusicInstrument
    {
        public int SheetMusicId { get; set; }
        public int InstrumentId { get; set; }

        public Instrument Instrument { get; set; }
        public SheetMusic SheetMusic { get; set; }
    }
}
