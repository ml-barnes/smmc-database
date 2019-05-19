using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class EnrollmentClass
    {
        public int EnrollmentId { get; set; }
        public int ClassId { get; set; }
        public DateTime Date { get; set; }

        public Class Class { get; set; }
        public Enrollment Enrollment { get; set; }
    }
}
