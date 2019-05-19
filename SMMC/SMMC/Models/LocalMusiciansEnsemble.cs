using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class LocalMusiciansEnsemble
    {
        public int LocalMusiciansEnsembleId { get; set; }
        public int LocalMusiciansId { get; set; }
        public int EnsembleId { get; set; }

        public Ensemble Ensemble { get; set; }
        public LocalMusicians LocalMusicians { get; set; }
    }
}
