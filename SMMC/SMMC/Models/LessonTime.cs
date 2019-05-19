using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class LessonTime
    {
        public LessonTime()
        {
            MusicClass = new HashSet<MusicClass>();
        }

        public int LessonTimeId { get; set; }
        public TimeSpan Time { get; set; }

        public ICollection<MusicClass> MusicClass { get; set; }
    }
}
