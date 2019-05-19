using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class EnsemblePerformance
    {
        public int EnsembleId { get; set; }
        public int PerformanceId { get; set; }

        public Ensemble Ensemble { get; set; }
        public Performance Performance { get; set; }
    }
}
