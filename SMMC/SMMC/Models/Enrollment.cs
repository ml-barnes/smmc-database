using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Enrollment
    {
        public Enrollment()
        {
            EnrollmentEnsemble = new HashSet<EnrollmentEnsemble>();
            EnrollmentMusicClass = new HashSet<EnrollmentMusicClass>();
            Loan = new HashSet<Loan>();
        }

        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int InstrumentId { get; set; }
        public int Grade { get; set; }
        public DateTime Date { get; set; }
        public bool Paid { get; set; }

        public Instrument Instrument { get; set; }
        public Student Student { get; set; }
        public ICollection<EnrollmentEnsemble> EnrollmentEnsemble { get; set; }
        public ICollection<EnrollmentMusicClass> EnrollmentMusicClass { get; set; }
        public ICollection<Loan> Loan { get; set; }
    }
}
