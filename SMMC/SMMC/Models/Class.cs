using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Class
    {
        public Class()
        {
            EnrollmentClass = new HashSet<EnrollmentClass>();
        }

        public int ClassId { get; set; }
        public int? TutorId { get; set; }
        public int? LessonTimeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public LessonTime LessonTime { get; set; }
        public Tutor Tutor { get; set; }
        public ICollection<EnrollmentClass> EnrollmentClass { get; set; }
    }
}
