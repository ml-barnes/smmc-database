using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Ensemble
    {
        public Ensemble()
        {
            EnrollmentEnsemble = new HashSet<EnrollmentEnsemble>();
            EnsemblePerformance = new HashSet<EnsemblePerformance>();
            LocalMusiciansEnsemble = new HashSet<LocalMusiciansEnsemble>();
        }

        public int EnsembleId { get; set; }
        public string Type { get; set; }

        public ICollection<EnrollmentEnsemble> EnrollmentEnsemble { get; set; }
        public ICollection<EnsemblePerformance> EnsemblePerformance { get; set; }
        public ICollection<LocalMusiciansEnsemble> LocalMusiciansEnsemble { get; set; }
    }
}
