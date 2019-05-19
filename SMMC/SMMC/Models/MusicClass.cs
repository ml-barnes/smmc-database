using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class MusicClass
    {
        public MusicClass()
        {
            EnrollmentMusicClass = new HashSet<EnrollmentMusicClass>();
        }

        public int MusicClassId { get; set; }
        public int? TutorId { get; set; }
        public int? LessonTimeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public LessonTime LessonTime { get; set; }
        public Tutor Tutor { get; set; }
        public ICollection<EnrollmentMusicClass> EnrollmentMusicClass { get; set; }
    }
}
