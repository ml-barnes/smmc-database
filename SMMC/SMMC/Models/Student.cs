using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Student
    {
        public Student()
        {
            Enrollment = new HashSet<Enrollment>();
            StudentGuardian = new HashSet<StudentGuardian>();
        }

        public int StudentId { get; set; }
        public int PersonId { get; set; }
        public int? ContactId { get; set; }

        public Contact Contact { get; set; }
        public Person Person { get; set; }
        public ICollection<Enrollment> Enrollment { get; set; }
        public ICollection<StudentGuardian> StudentGuardian { get; set; }
    }
}
