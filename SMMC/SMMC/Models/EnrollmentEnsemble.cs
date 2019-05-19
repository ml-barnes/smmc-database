using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class EnrollmentEnsemble
    {
        public int EnrollmentId { get; set; }
        public int EnsembleId { get; set; }

        public Enrollment Enrollment { get; set; }
        public Ensemble Ensemble { get; set; }
    }
}
