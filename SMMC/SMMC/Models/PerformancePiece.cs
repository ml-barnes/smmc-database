using System;
using System.Collections.Generic;

namespace SMMC.Models
{
    public partial class PerformancePiece
    {
        public int PerformancePieceId { get; set; }
        public int PerformanceId { get; set; }
        public int PieceId { get; set; }

        public Performance Performance { get; set; }
        public Piece Piece { get; set; }
    }
}
