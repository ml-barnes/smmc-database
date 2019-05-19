using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class SheetMusicTutor
    {
        public int SheetMusicTutorId { get; set; }
        public int SheetMusicId { get; set; }
        public int TutorId { get; set; }
        public int AmountLoaned { get; set; }
        public DateTime DateLoaned { get; set; }
        public DateTime? DateReturned { get; set; }

        public SheetMusic SheetMusic { get; set; }
        public Tutor Tutor { get; set; }
    }
}
