using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Instrument
    {
        public Instrument()
        {
            Enrollment = new HashSet<Enrollment>();
            InstrumentInventory = new HashSet<InstrumentInventory>();
            TutorType = new HashSet<TutorType>();
        }

        public int InstrumentId { get; set; }
        public string Instrument1 { get; set; }
        public int StudentFee { get; set; }
        public int? OpenFee { get; set; }
        public int? HireFee { get; set; }
        public int HeadTutor { get; set; }

        public Tutor HeadTutorNavigation { get; set; }
        public ICollection<Enrollment> Enrollment { get; set; }
        public ICollection<InstrumentInventory> InstrumentInventory { get; set; }
        public ICollection<TutorType> TutorType { get; set; }
    }
}
