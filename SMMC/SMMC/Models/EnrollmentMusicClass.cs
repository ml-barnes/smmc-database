using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class EnrollmentMusicClass
    {
        public int EnrollmentId { get; set; }
        public int MusicClassId { get; set; }
        public DateTime Date { get; set; }

        public Enrollment Enrollment { get; set; }
        public MusicClass MusicClass { get; set; }
    }
}
