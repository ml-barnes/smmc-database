using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class TutorType
    {
        public int TutorTypeId { get; set; }
        public int TutorId { get; set; }
        public int InstrumentId { get; set; }
        public int MaxGrade { get; set; }

        public Instrument Instrument { get; set; }
        public Tutor Tutor { get; set; }
    }
}
