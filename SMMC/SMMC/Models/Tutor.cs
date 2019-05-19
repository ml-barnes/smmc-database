using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Tutor
    {
        public Tutor()
        {
            Instrument = new HashSet<Instrument>();
            MusicClass = new HashSet<MusicClass>();
            SheetMusicTutor = new HashSet<SheetMusicTutor>();
            TutorType = new HashSet<TutorType>();
        }

        public int TutorId { get; set; }
        public int StaffId { get; set; }
        public bool Atcl { get; set; }

        public Staff Staff { get; set; }
        public ICollection<Instrument> Instrument { get; set; }
        public ICollection<MusicClass> MusicClass { get; set; }
        public ICollection<SheetMusicTutor> SheetMusicTutor { get; set; }
        public ICollection<TutorType> TutorType { get; set; }
    }
}
