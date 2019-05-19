using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class Performance
    {
        public Performance()
        {
            EnsemblePerformance = new HashSet<EnsemblePerformance>();
            PerformancePiece = new HashSet<PerformancePiece>();
        }

        public int PerformanceId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int VenueId { get; set; }

        public Venue Venue { get; set; }
        public ICollection<EnsemblePerformance> EnsemblePerformance { get; set; }
        public ICollection<PerformancePiece> PerformancePiece { get; set; }
    }
}
