using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class StudentGuardian
    {
        public int StudentId { get; set; }
        public int GuardianId { get; set; }

        public Guardian Guardian { get; set; }
        public Student Student { get; set; }
    }
}
